using System;
using System.Collections.Generic;

namespace MissingPersons.DAL.Models;

public partial class Report
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

    public bool? IsVerified { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ItemDetail? ItemDetail { get; set; }

    public virtual ICollection<Match> MatchFoundReports { get; set; } = new List<Match>();

    public virtual ICollection<Match> MatchLostReports { get; set; } = new List<Match>();

    public virtual PersonDetail? PersonDetail { get; set; }

    public virtual ICollection<ReportFlag> ReportFlags { get; set; } = new List<ReportFlag>();

    public virtual ICollection<ReportImage> ReportImages { get; set; } = new List<ReportImage>();

    public virtual User? User { get; set; }
}
