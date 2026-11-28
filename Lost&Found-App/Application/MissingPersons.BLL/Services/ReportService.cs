namespace MissingPersons.BLL.Services;

public class ReportService : IReportService
{
    private readonly IReportRepository _repo;
    private readonly IValidator<CreateReportDto> _createValidator;
    private readonly IValidator<UpdateReportDto> _updateValidator;

    public ReportService(
        IReportRepository repo,
        IValidator<CreateReportDto> createValidator,
        IValidator<UpdateReportDto> updateValidator)
    {
        _repo = repo;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    // ================= Create =================
    public async Task<Result<int>> CreateAsync(CreateReportDto dto)
    {
        var validation = await _createValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result<int>.Failure(ToError(validation));

        if (dto.TargetType == ReportConstants.Item &&
            !await _repo.CategoryExistsAsync(dto.ItemDetail!.CategoryId))
            return Result<int>.Failure(
                new Error("Report.InvalidCategory", "Selected category does not exist."));

        var report = new Report
        {
            UserId = dto.UserId,
            ReportType = dto.ReportType,
            TargetType = dto.TargetType,
            Title = dto.Title.Trim(),
            Description = dto.Description.Trim(),
            EventDate = dto.EventDate,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            Address = dto.Address,
            City = dto.City,
            Status = ReportConstants.Pending,
            IsVerified = false,
            IsDeleted = false,
            CreatedAt = DateTime.Now
        };

        if (dto.TargetType == ReportConstants.Person)
        {
            report.PersonDetail = new PersonDetail();
            ApplyPerson(report.PersonDetail, dto.PersonDetail!);
        }
        else
        {
            report.ItemDetail = new ItemDetail();
            ApplyItem(report.ItemDetail, dto.ItemDetail!);
        }

        foreach (var img in dto.Images)
        {
            report.ReportImages.Add(new ReportImage
            {
                ImageUrl = img.ImageUrl,
                IsPrimary = img.IsPrimary,
                UploadedAt = DateTime.Now
            });
        }

        try
        {
            await _repo.AddAsync(report);
            await _repo.SaveChangesAsync(); // Transaction واحدة لكل شيء
            return Result<int>.Success(report.Id);
        }
        catch (DbUpdateException)
        {
            return Result<int>.Failure(
                new Error("Report.SaveFailed", "Could not save the report."));
        }
    }

    // ================= Read =================
    public async Task<Result<ReportDto>> GetByIdAsync(int id)
    {
        var report = await _repo.GetByIdWithDetailsAsync(id);
        return report == null
            ? Result<ReportDto>.Failure(NotFound)
            : Result<ReportDto>.Success(MapToDto(report));
    }

    public async Task<Result<List<ReportDto>>> GetMyReportsAsync(int userId)
    {
        var reports = await _repo.GetByUserIdAsync(userId);
        return Result<List<ReportDto>>.Success(reports.Select(MapToDto).ToList());
    }

    public async Task<Result<PagedResult<ReportDto>>> GetPagedAsync(ReportFilterDto filter)
    {
        var page = filter.Page < 1 ? 1 : filter.Page;
        var pageSize = Math.Clamp(filter.PageSize, 1, 50);

        var (items, total) = await _repo.GetPagedAsync(
            filter.ReportType, filter.TargetType, filter.Status,
            filter.City, page, pageSize);

        return Result<PagedResult<ReportDto>>.Success(new PagedResult<ReportDto>
        {
            Items = items.Select(MapToDto).ToList(),
            TotalCount = total,
            Page = page,
            PageSize = pageSize
        });
    }
    // ================= Update =================
    public async Task<Result> UpdateAsync(int id, int userId, UpdateReportDto dto)
    {
        var validation = await _updateValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return Result.Failure(ToError(validation));

        var report = await _repo.GetByIdWithDetailsAsync(id);
        if (report == null) return Result.Failure(NotFound);

        if (report.UserId != userId) return Result.Failure(Forbidden);

        if (report.Status is "Matched" or "Closed")
            return Result.Failure(
                new Error("Report.Locked", "A matched or closed report cannot be edited."));

        if (report.TargetType == ReportConstants.Item && dto.ItemDetail != null &&
            !await _repo.CategoryExistsAsync(dto.ItemDetail.CategoryId))
            return Result.Failure(
                new Error("Report.InvalidCategory", "Selected category does not exist."));

        report.Title = dto.Title.Trim();
        report.Description = dto.Description.Trim();
        report.EventDate = dto.EventDate;
        report.Latitude = dto.Latitude;
        report.Longitude = dto.Longitude;
        report.Address = dto.Address;
        report.City = dto.City;

        if (report.TargetType == ReportConstants.Person &&
            dto.PersonDetail != null && report.PersonDetail != null)
            ApplyPerson(report.PersonDetail, dto.PersonDetail);

        if (report.TargetType == ReportConstants.Item &&
            dto.ItemDetail != null && report.ItemDetail != null)
            ApplyItem(report.ItemDetail, dto.ItemDetail);

        try
        {
            _repo.Update(report);
            await _repo.SaveChangesAsync();
            return Result.Success();
        }
        catch (DbUpdateException)
        {
            return Result.Failure(
                new Error("Report.SaveFailed", "Could not update the report."));
        }
    }

    // ================= Delete (Soft) =================
    public async Task<Result> DeleteAsync(int id, int userId)
    {
        var report = await _repo.GetByIdAsync(id);
        if (report == null) return Result.Failure(NotFound);

        if (report.UserId != userId) return Result.Failure(Forbidden);

        _repo.SoftDelete(report);
        await _repo.SaveChangesAsync();
        return Result.Success();
    }

    // ================= Helpers =================
    private static readonly Error NotFound =
        new("Report.NotFound", "Report not found.");

    private static readonly Error Forbidden =
        new("Report.Forbidden", "You are not allowed to modify this report.");

    private static Error ToError(FluentValidation.Results.ValidationResult v) =>
        new("Validation", string.Join(" | ", v.Errors.Select(e => e.ErrorMessage)));

    private static void ApplyPerson(PersonDetail e, PersonDetailDto d)
    {
        e.PersonName = d.PersonName;
        e.Age = d.Age;
        e.Gender = d.Gender;
        e.HairColor = d.HairColor;
        e.EyeColor = d.EyeColor;
        e.Height = d.Height;
        e.ClothingDescription = d.ClothingDescription;
        e.DistinguishingMarks = d.DistinguishingMarks;
        e.MedicalCondition = d.MedicalCondition;
        e.ContactPhoneForFamily = d.ContactPhoneForFamily;
    }

    private static void ApplyItem(ItemDetail e, ItemDetailDto d)
    {
        e.CategoryId = d.CategoryId;
        e.Brand = d.Brand;
        e.Model = d.Model;
        e.Color = d.Color;
        e.SerialNumber = d.SerialNumber;
        e.Size = d.Size;
    }

    private static ReportDto MapToDto(Report r) => new()
    {
        Id = r.Id,
        UserId = r.UserId,
        ReportType = r.ReportType,
        TargetType = r.TargetType,
        Title = r.Title,
        Description = r.Description,
        EventDate = r.EventDate,
        Latitude = r.Latitude,
        Longitude = r.Longitude,
        Address = r.Address,
        City = r.City,
        Status = r.Status,
        CreatedAt = r.CreatedAt,
        PersonDetail = r.PersonDetail == null ? null : new PersonDetailDto
        {
            PersonName = r.PersonDetail.PersonName,
            Age = r.PersonDetail.Age,
            Gender = r.PersonDetail.Gender,
            HairColor = r.PersonDetail.HairColor,
            EyeColor = r.PersonDetail.EyeColor,
            Height = r.PersonDetail.Height,
            ClothingDescription = r.PersonDetail.ClothingDescription,
            DistinguishingMarks = r.PersonDetail.DistinguishingMarks,
            MedicalCondition = r.PersonDetail.MedicalCondition,
            ContactPhoneForFamily = r.PersonDetail.ContactPhoneForFamily
        },
        ItemDetail = r.ItemDetail == null ? null : new ItemDetailDto
        {
            CategoryId = r.ItemDetail.CategoryId ?? 0,
            Brand = r.ItemDetail.Brand,
            Model = r.ItemDetail.Model,
            Color = r.ItemDetail.Color,
            SerialNumber = r.ItemDetail.SerialNumber,
            Size = r.ItemDetail.Size
        },
        Images = r.ReportImages
            .OrderByDescending(i => i.IsPrimary)
            .Select(i => new ReportImageDto
            {
                ImageUrl = i.ImageUrl,
                IsPrimary = i.IsPrimary ?? false
            }).ToList()
    };
}
