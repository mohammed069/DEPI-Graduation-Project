using System;
using System.Collections.Generic;

namespace QuizApp.Models;

public partial class Quiz
{
    public int QuizId { get; set; }

    public string Title { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public int Duration { get; set; }

    public decimal TotalMarks { get; set; }

    public int CourseId { get; set; }

    public int CreatedBy { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
