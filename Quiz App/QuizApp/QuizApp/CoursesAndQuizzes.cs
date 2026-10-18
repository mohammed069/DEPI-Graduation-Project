using Microsoft.EntityFrameworkCore;
using QuizApp.Data;

namespace QuizApp;

public static class CoursesAndQuizzes {
    public static async Task ShowAsync(AppDbContext db) {
        var courses = await db.Courses
            .AsNoTracking()
            .Include(c => c.Quizzes)
            .OrderBy(c => c.CourseId)
            .ToListAsync();

        if(courses.Count == 0) {
            Console.WriteLine("No courses available.");
            return;
        }

        Console.WriteLine("===== Available Courses =====");
        foreach(var course in courses) {
            Console.WriteLine($"\n[{course.CourseId}] {course.Title}");
            if(!string.IsNullOrWhiteSpace(course.Description))
                Console.WriteLine($"    {course.Description}");

            if(course.Quizzes.Count == 0) {
                Console.WriteLine("    (no quizzes yet)");
                continue;
            }

            foreach(var q in course.Quizzes.OrderBy(q => q.StartTime))
                Console.WriteLine(
                    $"    - Quiz #{q.QuizId}: {q.Title} | Start: {q.StartTime:yyyy-MM-dd HH:mm} | Duration: {q.Duration} | Marks: {q.TotalMarks}");
        }
    }
}