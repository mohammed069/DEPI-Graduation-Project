using System;
using System.Collections.Generic;

namespace QuizApp.Models;

public partial class Question
{
    public int QuestionId { get; set; }

    public string QuestionText { get; set; } = null!;

    public string Type { get; set; } = null!;

    public decimal Marks { get; set; }

    public int? CourseId { get; set; }

    public virtual Course? Course { get; set; }

    public virtual Option? Option { get; set; }

    public virtual ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();

    public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
}
