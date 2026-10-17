using System;
using System.Collections.Generic;

namespace QuizApp.Models;

public partial class StudentAnswer
{
    public int AnswerId { get; set; }

    public bool? IsCorrect { get; set; }

    public decimal MarksAwarded { get; set; }

    public int AttemptId { get; set; }

    public int QuestionId { get; set; }

    public int? OptionId { get; set; }

    public virtual QuizAttempt Attempt { get; set; } = null!;

    public virtual Option? Option { get; set; }

    public virtual Question Question { get; set; } = null!;
}
