namespace MissingPersons.BLL.DTOs;

public class PersonDetailDto
{
    public string? PersonName { get; set; }
    public int? Age { get; set; }
    public string? Gender { get; set; }
    public string? HairColor { get; set; }
    public string? EyeColor { get; set; }
    public double? Height { get; set; }
    public string? ClothingDescription { get; set; }
    public string? DistinguishingMarks { get; set; }
    public string? MedicalCondition { get; set; }
    public string? ContactPhoneForFamily { get; set; }
}

public class ItemDetailDto
{
    public int CategoryId { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public string? Color { get; set; }
    public string? SerialNumber { get; set; }
    public string? Size { get; set; }
}

public class ReportImageDto
{
    public string ImageUrl { get; set; } = null!;
    public bool IsPrimary { get; set; }
}

public class CreateReportDto
{
    public int UserId { get; set; }
    public string ReportType { get; set; } = null!;   // Lost / Found
    public string TargetType { get; set; } = null!;   // Person / Item
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime EventDate { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }

    public PersonDetailDto? PersonDetail { get; set; }
    public ItemDetailDto? ItemDetail { get; set; }
    public List<ReportImageDto> Images { get; set; } = new();
}

public class ReportDto
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public string? ReportType { get; set; }
    public string? TargetType { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime? EventDate { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Status { get; set; }
    public DateTime? CreatedAt { get; set; }

    public PersonDetailDto? PersonDetail { get; set; }
    public ItemDetailDto? ItemDetail { get; set; }
    public List<ReportImageDto> Images { get; set; } = new();
}

public class ReportFilterDto
{
    public string? ReportType { get; set; }
    public string? TargetType { get; set; }
    public string? Status { get; set; }
    public string? City { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
public class UpdateReportDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime EventDate { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }

    public PersonDetailDto? PersonDetail { get; set; }
    public ItemDetailDto? ItemDetail { get; set; }
}