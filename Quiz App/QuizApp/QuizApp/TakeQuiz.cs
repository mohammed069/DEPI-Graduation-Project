using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.Models;
namespace QuizApp;

public static class TakeQuiz
{
    public static async Task RunAsync(AppDbContext db)
    {
        Console.Clear();
        Console.WriteLine("===== Take a Quiz =====");
        Console.WriteLine();

        
        Console.Write("Enter Student ID: ");

        if (!int.TryParse(Console.ReadLine(), out int studentId))
        {
            Console.WriteLine("Invalid Student ID.");
            return;
        }

        var student = await db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u =>
                u.UserId == studentId &&
                u.Role == "Student");

        if (student == null)
        {
            Console.WriteLine("Student not found.");
            return;
        }

      
        Console.Write("Enter Quiz ID: ");

        if (!int.TryParse(Console.ReadLine(), out int quizId))
        {
            Console.WriteLine("Invalid Quiz ID.");
            return;
        }

        var quiz = await db.Quizzes
            .Include(q => q.Questions)
            .FirstOrDefaultAsync(q => q.QuizId == quizId);

        if (quiz == null)
        {
            Console.WriteLine("Quiz not found.");
            return;
        }

         
        bool alreadyTaken = await db.QuizAttempts
            .AnyAsync(a =>
                a.UserId == studentId &&
                a.QuizId == quizId);

        if (alreadyTaken)
        {
            Console.WriteLine();
            Console.WriteLine("You have already taken this quiz.");
            return;
        }

        if (quiz.Questions.Count == 0)
        {
            Console.WriteLine();
            Console.WriteLine("This quiz has no questions.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine($"Quiz: {quiz.Title}");
        Console.WriteLine($"Total Marks: {quiz.TotalMarks}");
        Console.WriteLine($"Duration: {quiz.Duration} minutes");
        Console.WriteLine();

        Console.WriteLine("Press any key to start...");
        Console.ReadKey();

        DateTime startedAt = DateTime.Now;

         

        int attemptId = await db.QuizAttempts.AnyAsync()
            ? await db.QuizAttempts.MaxAsync(a => a.AttemptId) + 1
            : 1;

        var attempt = new QuizAttempt
        {
            AttemptId = attemptId,
            UserId = studentId,
            QuizId = quizId,
            StartedAt = startedAt,
            Score = 0
        };

        decimal score = 0;

        int nextAnswerId = await db.StudentAnswers.AnyAsync()
            ? await db.StudentAnswers.MaxAsync(a => a.AnswerId) + 1
            : 1;

        int questionNumber = 1;

        foreach (var question in quiz.Questions.OrderBy(q => q.QuestionId))
        {
            Console.Clear();

            Console.WriteLine($"Quiz: {quiz.Title}");
            Console.WriteLine(
                $"Question {questionNumber} of {quiz.Questions.Count}");

            Console.WriteLine();

            Console.WriteLine(question.QuestionText);
            Console.WriteLine($"Marks: {question.Marks}");
            Console.WriteLine();

            var options = await db.Options
                .AsNoTracking()
                .Where(o => o.QuestionId == question.QuestionId)
                .OrderBy(o => o.OptionId)
                .ToListAsync();

            if (options.Count == 0)
            {
                Console.WriteLine("No options found for this question.");
                questionNumber++;
                continue;
            }

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine(
                    $"{i + 1}. {options[i].OptionText}");
            }

            Console.WriteLine();

            int selectedNumber;

            while (true)
            {
                Console.Write("Enter your answer: ");

                if (int.TryParse(
                        Console.ReadLine(),
                        out selectedNumber)
                    &&
                    selectedNumber >= 1
                    &&
                    selectedNumber <= options.Count)
                {
                    break;
                }

                Console.WriteLine(
                    "Invalid choice. Please try again.");
            }

            Option selectedOption =
                options[selectedNumber - 1];

            bool isCorrect = selectedOption.
    IsCorrect;

decimal marksAwarded =
    isCorrect ? question.Marks : 0;

score += marksAwarded;

var studentAnswer = new StudentAnswer
{
    AnswerId = nextAnswerId,
    AttemptId = attemptId,
    QuestionId = question.QuestionId,
    OptionId = selectedOption.OptionId,
    IsCorrect = isCorrect,
    MarksAwarded = marksAwarded
};

attempt.StudentAnswers.Add(studentAnswer);

nextAnswerId++;
questionNumber++;
    }

    attempt.Score = score;
attempt.SubmittedAt = DateTime.Now;

db.QuizAttempts.Add(attempt);

await db.SaveChangesAsync();

Console.Clear();

Console.WriteLine("===== Quiz Finished =====");
Console.WriteLine();

Console.WriteLine($"Student: {student.Name}");
Console.WriteLine($"Quiz: {quiz.Title}");
Console.WriteLine($"Score: {score} / {quiz.TotalMarks}");

if (quiz.TotalMarks > 0)
{
    decimal percentage =
        (score / quiz.TotalMarks) * 100;

    Console.WriteLine(
        $"Percentage: {percentage:F2}%");
}

Console.WriteLine();
Console.WriteLine("Your answers were saved successfully.");
}
}