using System;
using System.Collections.Generic;

namespace MissingPersons.DAL.Models;

public partial class UserVerification
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? Method { get; set; }

    public string? Status { get; set; }

    public string? DocumentUrl { get; set; }

    public int? ReviewedByAdminId { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public virtual User? ReviewedByAdmin { get; set; }

    public virtual User? User { get; set; }
}
