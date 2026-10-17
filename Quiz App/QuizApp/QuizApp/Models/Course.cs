using System;
using System.Collections.Generic;

namespace QuizApp.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();

    public virtual ICollection<UserCourseEnrollment> UserCourseEnrollments { get; set; } = new List<UserCourseEnrollment>();

    public virtual ICollection<UserCourseTeach> UserCourseTeaches { get; set; } = new List<UserCourseTeach>();
}
