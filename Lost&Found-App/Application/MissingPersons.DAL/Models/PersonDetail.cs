using System;
using System.Collections.Generic;

namespace MissingPersons.DAL.Models;

public partial class PersonDetail
{
    public int ReportId { get; set; }

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

    public virtual Report Report { get; set; } = null!;
}
