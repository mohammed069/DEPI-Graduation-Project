using System;
using System.Collections.Generic;

namespace QuizApp.Models;

public partial class UserCourseEnrollment
{
    public int EnrollmentId { get; set; }

    public int UserId { get; set; }

    public int CourseId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
