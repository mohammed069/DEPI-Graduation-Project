using System;
using System.Collections.Generic;

namespace MissingPersons.DAL.Models;

public partial class ReportImage
{
    public int Id { get; set; }

    public int? ReportId { get; set; }

    public string? ImageUrl { get; set; }

    public bool? IsPrimary { get; set; }

    public byte[]? FaceEmbedding { get; set; }

    public string? ImageHash { get; set; }

    public DateTime? UploadedAt { get; set; }

    public virtual Report? Report { get; set; }
}
