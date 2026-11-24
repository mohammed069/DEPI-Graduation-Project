using System;
using System.Collections.Generic;

namespace MissingPersons.DAL.Models;

public partial class UserLocationPreference
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public double? RadiusKm { get; set; }

    public bool? NotifyEnabled { get; set; }

    public virtual User? User { get; set; }
}
