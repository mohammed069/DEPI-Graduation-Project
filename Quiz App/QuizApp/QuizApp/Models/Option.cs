using System;
using System.Collections.Generic;

namespace QuizApp.Models;

public partial class Option
{
    public int OptionId { get; set; }

    public string OptionText { get; set; } = null!;

    public bool IsCorrect { get; set; }

    public int QuestionId { get; set; }

    public virtual Question Question { get; set; } = null!;

    public virtual ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
}
