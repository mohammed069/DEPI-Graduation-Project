using System;
using System.Collections.Generic;

namespace MissingPersons.DAL.Models;

public partial class ReportFlag
{
    public int Id { get; set; }

    public int? ReportId { get; set; }

    public int? ReportedByUserId { get; set; }

    public string Reason { get; set; } = null!;

    public string? Status { get; set; }

    public int? ReviewedByAdminId { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public virtual Report? Report { get; set; }

    public virtual User? ReportedByUser { get; set; }

    public virtual User? ReviewedByAdmin { get; set; }
}
