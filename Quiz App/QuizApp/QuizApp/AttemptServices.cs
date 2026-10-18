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
}





//public static class AttemptServices
//{
//    // 1. Start a new quiz attempt
//    public static async Task<int> StartAsync(
//        AppDbContext db,
//        int userId,
//        int quizId)
//    {
//        int lastAttemptId = await db.QuizAttempts
//            .MaxAsync(a => (int?)a.AttemptId) ?? 0;

//        int newAttemptId = lastAttemptId + 1;

//        var attempt = new QuizAttempt
//        {
//            AttemptId = newAttemptId,
//            UserId = userId,
//            QuizId = quizId,
//            StartedAt = DateTime.Now,
//            Score = 0
//        };

//        db.QuizAttempts.Add(attempt);
//        await db.SaveChangesAsync();

//        return newAttemptId;
//    }

//    // 2. Save the student's answer
//    public static async Task SaveAnswerAsync(
//        AppDbContext db,
//        int attemptId,
//        int questionId,
//        int? optionId)
//    {
//        // Check if this question already has an answer
//        var existingAnswer = await db.StudentAnswers
//            .FirstOrDefaultAsync(a =>
//                a.AttemptId == attemptId &&
//                a.QuestionId == questionId);

//        if (existingAnswer != null)
//        {
//            // Update the existing answer
//            existingAnswer.OptionId = optionId;
//            existingAnswer.IsCorrect = null;
//            existingAnswer.MarksAwarded = 0;
//        }
//        else
//        {
//            // Generate a new answer ID
//            int lastAnswerId = await db.StudentAnswers
//                .MaxAsync(a => (int?)a.AnswerId) ?? 0;

//            int newAnswerId = lastAnswerId + 1;

//            var answer = new StudentAnswer
//            {
//                AnswerId = newAnswerId,
//                AttemptId = attemptId,
//                QuestionId = questionId,
//                OptionId = optionId,
//                IsCorrect = null,
//                MarksAwarded = 0
//            };

//            db.StudentAnswers.Add(answer);
//        }

//        await db.SaveChangesAsync();
//    }

//    // 3. Submit the quiz only when all questions are answered
//    public static async Task<bool> SubmitAsync(
//        AppDbContext db,
//        int attemptId)
//    {
//        var attempt = await db.QuizAttempts
//            .Include(a => a.Quiz)
//            .ThenInclude(q => q.Questions)
//            .FirstOrDefaultAsync(a => a.AttemptId == attemptId);

//        if (attempt == null)
//        {
//            Console.WriteLine("Attempt not found.");
//            return false;
//        }

//        if (attempt.SubmittedAt != null)
//        {
//            Console.WriteLine("This quiz has already been submitted.");
//            return false;
//        }

//        var questionIds = attempt.Quiz.Questions
//            .Select(q => q.QuestionId)
//            .ToList();

//        var answeredQuestionIds = await db.StudentAnswers
//            .Where(a => a.AttemptId == attemptId && a.OptionId.HasValue)  
//            .Select(a => a.QuestionId)
//            .ToListAsync();

//        bool hasUnansweredQuestions = questionIds
//            .Any(id => !answeredQuestionIds.Contains(id));

//        if (hasUnansweredQuestions)
//        {
//            Console.WriteLine(
//                "You must answer all questions before submitting.");

//            return false;
//        }

//        attempt.SubmittedAt = DateTime.Now;

//        await db.SaveChangesAsync();

//        Console.WriteLine("Quiz submitted successfully!");

//        return true;
//    }

//}
