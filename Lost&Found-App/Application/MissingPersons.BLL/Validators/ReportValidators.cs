namespace MissingPersons.BLL.Validators;

public class CreateReportDtoValidator : AbstractValidator<CreateReportDto>
{
    private static readonly string[] AllowedGenders = { "Male", "Female", "Unknown" };

    public CreateReportDtoValidator()
    {
        // ===== بيانات البلاغ الأساسية =====
        RuleFor(x => x.UserId).GreaterThan(0)
            .WithMessage("User is required.");

        RuleFor(x => x.ReportType)
            .Must(t => t == ReportConstants.Lost || t == ReportConstants.Found)
            .WithMessage("ReportType must be Lost or Found.");

        RuleFor(x => x.TargetType)
            .Must(t => t == ReportConstants.Person || t == ReportConstants.Item)
            .WithMessage("TargetType must be Person or Item.");

        RuleFor(x => x.Title)
            .NotEmpty().MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty().Must(d => !string.IsNullOrWhiteSpace(d))
            .WithMessage("Description cannot be empty.");

        RuleFor(x => x.EventDate)
            .LessThanOrEqualTo(_ => DateTime.Now)
            .WithMessage("Event date cannot be in the future.");

        // ===== الموقع =====
        RuleFor(x => x.Latitude).InclusiveBetween(-90, 90)
            .When(x => x.Latitude.HasValue);

        RuleFor(x => x.Longitude).InclusiveBetween(-180, 180)
            .When(x => x.Longitude.HasValue);

        RuleFor(x => x)
            .Must(x => x.Latitude.HasValue == x.Longitude.HasValue)
            .WithMessage("Latitude and Longitude must be provided together.");

        // ===== قاعدة: Person أو Item مش الاتنين =====
        RuleFor(x => x.PersonDetail).NotNull()
            .When(x => x.TargetType == ReportConstants.Person)
            .WithMessage("Person details are required.");

        RuleFor(x => x.ItemDetail).Null()
            .When(x => x.TargetType == ReportConstants.Person)
            .WithMessage("Item details are not allowed for a Person report.");

        RuleFor(x => x.ItemDetail).NotNull()
            .When(x => x.TargetType == ReportConstants.Item)
            .WithMessage("Item details are required.");

        RuleFor(x => x.PersonDetail).Null()
            .When(x => x.TargetType == ReportConstants.Item)
            .WithMessage("Person details are not allowed for an Item report.");

        // ===== التفاصيل =====
        RuleFor(x => x.PersonDetail!)
            .SetValidator(new PersonDetailDtoValidator())
            .When(x => x.PersonDetail != null);

        RuleFor(x => x.ItemDetail!)
            .SetValidator(new ItemDetailDtoValidator())
            .When(x => x.ItemDetail != null);

        // الاسم مطلوب لو شخص مفقود (Lost)، اختياري لو شخص لقيناه (Found)
        RuleFor(x => x.PersonDetail!.PersonName).NotEmpty()
            .When(x => x.TargetType == ReportConstants.Person
                    && x.ReportType == ReportConstants.Lost
                    && x.PersonDetail != null)
            .WithMessage("Person name is required for a missing person.");

        // ===== الصور =====
        RuleFor(x => x.Images)
            .Must(i => i.Count <= 5)
            .WithMessage("Maximum 5 images.");

        RuleFor(x => x.Images)
            .Must(i => i.Count(img => img.IsPrimary) == 1)
            .When(x => x.Images.Count > 0)
            .WithMessage("Exactly one image must be primary.");

        RuleForEach(x => x.Images)
            .SetValidator(new ReportImageDtoValidator());
    }
}

public class PersonDetailDtoValidator : AbstractValidator<PersonDetailDto>
{
    private static readonly string[] AllowedGenders = { "Male", "Female", "Unknown" };

    public PersonDetailDtoValidator()
    {
        RuleFor(x => x.Age).InclusiveBetween(0, 120)
            .When(x => x.Age.HasValue);

        RuleFor(x => x.Height).GreaterThan(0)
            .When(x => x.Height.HasValue);

        RuleFor(x => x.Gender)
            .Must(g => AllowedGenders.Contains(g))
            .When(x => !string.IsNullOrEmpty(x.Gender))
            .WithMessage("Gender must be Male, Female or Unknown.");
        RuleFor(x => x.ContactPhoneForFamily)
            .Matches(@"^\+?[0-9]{10,15}$")
            .When(x => !string.IsNullOrEmpty(x.ContactPhoneForFamily))
            .WithMessage("Invalid phone number.");
    }
}

public class ItemDetailDtoValidator : AbstractValidator<ItemDetailDto>
{
    public ItemDetailDtoValidator()
    {
        RuleFor(x => x.CategoryId).GreaterThan(0)
            .WithMessage("Category is required.");

        RuleFor(x => x.SerialNumber)
            .Must(s => !string.IsNullOrWhiteSpace(s))
            .When(x => x.SerialNumber != null)
            .WithMessage("Serial number cannot be empty.");
    }
}

public class ReportImageDtoValidator : AbstractValidator<ReportImageDto>
{
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

    public ReportImageDtoValidator()
    {
        RuleFor(x => x.ImageUrl).NotEmpty();

        RuleFor(x => x.ImageUrl)
            .Must(url => AllowedExtensions.Contains(
                Path.GetExtension(url).ToLowerInvariant()))
            .When(x => !string.IsNullOrEmpty(x.ImageUrl))
            .WithMessage("Image must be jpg, jpeg, png or webp.");
    }
    public class UpdateReportDtoValidator : AbstractValidator<UpdateReportDto>
    {
        public UpdateReportDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);

            RuleFor(x => x.Description)
                .NotEmpty().Must(d => !string.IsNullOrWhiteSpace(d))
                .WithMessage("Description cannot be empty.");

            RuleFor(x => x.EventDate)
                .LessThanOrEqualTo(_ => DateTime.Now)
                .WithMessage("Event date cannot be in the future.");

            RuleFor(x => x.Latitude).InclusiveBetween(-90, 90)
                .When(x => x.Latitude.HasValue);
            RuleFor(x => x.Longitude).InclusiveBetween(-180, 180)
                .When(x => x.Longitude.HasValue);
            RuleFor(x => x)
                .Must(x => x.Latitude.HasValue == x.Longitude.HasValue)
                .WithMessage("Latitude and Longitude must be provided together.");

            RuleFor(x => x.PersonDetail!)
                .SetValidator(new PersonDetailDtoValidator())
                .When(x => x.PersonDetail != null);
            RuleFor(x => x.ItemDetail!)
                .SetValidator(new ItemDetailDtoValidator())
                .When(x => x.ItemDetail != null);
        }
    }
}
