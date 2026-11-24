using System;
using System.Collections.Generic;

namespace MissingPersons.DAL.Models;

public partial class Match
{
    public int Id { get; set; }

    public int? LostReportId { get; set; }

    public int? FoundReportId { get; set; }

    public double? TotalScore { get; set; }

    public double? LocationScore { get; set; }

    public double? DateScore { get; set; }

    public double? DescriptionScore { get; set; }

    public double? ImageScore { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ConfirmedAt { get; set; }

    public virtual Conversation? Conversation { get; set; }

    public virtual Report? FoundReport { get; set; }

    public virtual Report? LostReport { get; set; }
}
