using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;

namespace QuizApp;

public static class AttemptServices
{
    // 1. Update an existing student answer
    public static async Task<bool> UpdateAnswerAsync(
        AppDbContext db,
        int attemptId,
        int questionId,
        int optionId)
    {
        var attempt = await db.QuizAttempts
            .FirstOrDefaultAsync(a => a.AttemptId == attemptId);

        if (attempt == null || attempt.SubmittedAt != null)
        {
            Console.WriteLine("Invalid or submitted attempt.");
            return false;
        }

        bool optionExists = await db.Options.AnyAsync(o =>
            o.OptionId == optionId &&
            o.QuestionId == questionId);

        if (!optionExists)
        {
            Console.WriteLine("Invalid option for this question.");
            return false;
        }

        var answer = await db.StudentAnswers
            .FirstOrDefaultAsync(a =>
                a.AttemptId == attemptId &&
                a.QuestionId == questionId);

        if (answer == null)
        {
            Console.WriteLine("No saved answer found to update.");
            return false;
        }

        answer.OptionId = optionId;

        // The grading service will determine correctness later.
        answer.IsCorrect = null;
        answer.MarksAwarded = 0;

        await db.SaveChangesAsync();

        return true;
    }

    // 2. Check whether all quiz questions have answers
    public static async Task<bool> AreAllQuestionsAnsweredAsync(
        AppDbContext db,
        int attemptId)
    {
        var attempt = await db.QuizAttempts
            .Include(a => a.Quiz)
            .ThenInclude(q => q.Questions)
            .FirstOrDefaultAsync(a => a.AttemptId == attemptId);

        if (attempt == null)
            return false;

        var questionIds = attempt.Quiz.Questions
            .Select(q => q.QuestionId)
            .ToList();

        var answeredQuestionIds = await db.StudentAnswers
            .Where(a => a.AttemptId == attemptId &&
                        a.OptionId.HasValue)
            .Select(a => a.QuestionId)
            .Distinct()
            .ToListAsync();

        return questionIds.All(
            id => answeredQuestionIds.Contains(id));
    }

    // 3. Check whether an attempt can be submitted
    public static async Task<bool> CanSubmitAsync(
        AppDbContext db,
        int attemptId)
    {
        var attemptExists = await db.QuizAttempts
            .AnyAsync(a =>
                a.AttemptId == attemptId &&
                a.SubmittedAt == null);

        if (!attemptExists)
            return false;

        return await AreAllQuestionsAnsweredAsync(db, attemptId);
    }
    // 4. Display a student's quiz results
    public static async Task viewMyResultsAsync(
        AppDbContext db,int studentId)
    {
        Console.Clear();
        var student = await db.Users
            .FirstOrDefaultAsync(u => u.UserId == studentId);
        if (student== null)
        {
            Console.WriteLine("student not found");
            return;
        }
        var attempts = await db.QuizAttempts
           .Include(a => a.Quiz)
           .Where(a => a.UserId == studentId && a.SubmittedAt != null)
           .OrderByDescending(a => a.SubmittedAt).ToListAsync();
        Console.ForegroundColor = ConsoleColor.Green;
       
        Console.WriteLine("=================================");
        Console.WriteLine("     Quiz Results     ");
        Console.WriteLine("==================================");
        Console.ResetColor();
        Console.WriteLine($"Student: {student.Name}");
        

        if (attempts.Count == 0)
        {
            Console.WriteLine("You have no completed quizzes yet.");
            return;
        }

        foreach (var attempt in attempts)
        {
            Console.WriteLine($"Quiz: {attempt.Quiz.Title}");
            Console.WriteLine($"Score: {attempt.Score} / {attempt.Quiz.TotalMarks}");

            Console.WriteLine($"Submitted At: {attempt.SubmittedAt:dd/MM/yyyy hh:mm tt}");

            Console.WriteLine("----------------------------------------");
        }


    }

}

