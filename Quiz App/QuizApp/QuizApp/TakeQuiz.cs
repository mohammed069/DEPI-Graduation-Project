using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.Models;
namespace QuizApp;

public static class TakeQuiz
{
    public static async Task RunAsync(AppDbContext db)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Clear(); Console.WriteLine("===== Take a Quiz =====");
        Console.ResetColor();
        Console.Write("Enter Student ID: ");
        int studentId = Convert.ToInt32(Console.ReadLine());

        var student = await db.Users
            .FirstOrDefaultAsync(x => x.UserId == studentId);

        if (student == null)
        {
            Console.WriteLine("Student not found.");
            return;
        }

        Console.Write("Enter Quiz ID: ");
        int quizId = Convert.ToInt32(Console.ReadLine());

        var quiz = await db.Quizzes
            .Include(x => x.Questions)
            .FirstOrDefaultAsync(x => x.QuizId == quizId);

        if (quiz == null)
        {
            Console.WriteLine("Quiz not found.");
            return;
        }

        var oldAttempt = await db.QuizAttempts
            .FirstOrDefaultAsync(x =>
                x.UserId == studentId &&
                x.QuizId == quizId);

        if (oldAttempt != null)
        {
            Console.WriteLine("You already took this quiz.");
            return;
        }

        int attemptId = 1;

        if (await db.QuizAttempts.AnyAsync())
        {
            attemptId =
                await db.QuizAttempts.MaxAsync(x => x.AttemptId) + 1;
        }

        int answerId = 1;

        if (await db.StudentAnswers.AnyAsync())
        {
            answerId =
                await db.StudentAnswers.MaxAsync(x => x.AnswerId) + 1;
        }

        var attempt = new QuizAttempt();

        attempt.AttemptId = attemptId;
        attempt.UserId = studentId;
        attempt.QuizId = quizId;
        attempt.StartedAt = DateTime.Now;
        attempt.Score = 0;

        db.QuizAttempts.Add(attempt);
        await db.SaveChangesAsync();

        decimal score = 0;

        foreach (var question in quiz.Questions)
        {
            Console.WriteLine();
            Console.WriteLine(question.QuestionText);

            var options = await db.Options
                .Where(x => x.QuestionId == question.QuestionId)
                .ToListAsync();

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine(
                    $"{i + 1}. {options[i].OptionText}");
            }

            Console.Write("Your answer: ");

            int answer =
                Convert.ToInt32(Console.ReadLine());

            if (answer < 1 || answer > options.Count)
            {
                Console.WriteLine("Invalid answer.");
                return;
            }

            var selectedOption = options[answer - 1];

            bool correct = selectedOption.IsCorrect;

            decimal marks = 0;

            if (correct)
            {
                marks = question.Marks;
                score += marks;
            }

            var studentAnswer = new StudentAnswer();

            studentAnswer.AnswerId = answerId;
            studentAnswer.AttemptId = attemptId;
            studentAnswer.QuestionId = question.QuestionId;
            studentAnswer.OptionId = selectedOption.OptionId;
            studentAnswer.IsCorrect = correct;
            studentAnswer.MarksAwarded = marks;

            db.StudentAnswers.Add(studentAnswer);

            answerId++;
        }



        attempt.Score = score;

        // Save answers before checking them
        await db.SaveChangesAsync();

        bool allAnswered =
            await AttemptServices.AreAllQuestionsAnsweredAsync(
                db, attemptId);



        if (!allAnswered)
        {
            Console.WriteLine("You must answer all questions before submitting.");
            return;
        }

        bool canSubmit =
            await AttemptServices.CanSubmitAsync(
                db, attemptId);

        if (!canSubmit)
        {
            Console.WriteLine("Cannot submit the quiz.");
            return;
        }

        attempt.SubmittedAt = DateTime.Now;

        await db.SaveChangesAsync();


        Console.WriteLine();
        Console.WriteLine("===== Quiz Finished =====");
        Console.WriteLine($"Student: {student.Name}");
        Console.WriteLine($"Quiz: {quiz.Title}");
        Console.WriteLine(
            $"Score: {score} / {quiz.TotalMarks}");
    }
}