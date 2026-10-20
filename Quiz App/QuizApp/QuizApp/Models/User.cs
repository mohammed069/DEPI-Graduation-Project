using System;
using System.Collections.Generic;

namespace QuizApp.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? Username { get; set; }

    public string? PasswordHash { get; set; }

    public virtual ICollection<Quiz> Quizzes { get; set; }
    = new List<Quiz>();

    public virtual ICollection<QuizAttempt> QuizAttempts { get; set; }
        = new List<QuizAttempt>();

    public virtual ICollection<UserCourseEnrollment> UserCourseEnrollments { get; set; }
        = new List<UserCourseEnrollment>();

    public virtual ICollection<UserCourseTeach> UserCourseTeaches { get; set; }
        = new List<UserCourseTeach>();
}
