using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
namespace MissingPersons.DAL.Models;

public partial class User:IdentityUser<int>
{
   

    public string? FullName { get; set; }

    public string? ProfileImageUrl { get; set; }

    public bool? IsVerified { get; set; }

    public bool? IsBlocked { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<ReportFlag> ReportFlagReportedByUsers { get; set; } = new List<ReportFlag>();

    public virtual ICollection<ReportFlag> ReportFlagReviewedByAdmins { get; set; } = new List<ReportFlag>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual UserLocationPreference? UserLocationPreference { get; set; }

    public virtual ICollection<UserVerification> UserVerificationReviewedByAdmins { get; set; } = new List<UserVerification>();

    public virtual ICollection<UserVerification> UserVerificationUsers { get; set; } = new List<UserVerification>();
}
