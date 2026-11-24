using System;
using System.Collections.Generic;

namespace MissingPersons.DAL.Models;

public partial class ItemDetail
{
    public int ReportId { get; set; }

    public int? CategoryId { get; set; }

    public string? Brand { get; set; }

    public string? Model { get; set; }

    public string? Color { get; set; }

    public string? SerialNumber { get; set; }

    public string? Size { get; set; }

    public virtual ItemCategory? Category { get; set; }

    public virtual Report Report { get; set; } = null!;
}
