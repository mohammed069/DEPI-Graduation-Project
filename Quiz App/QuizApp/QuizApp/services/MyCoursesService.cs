
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;

namespace QuizApp.Services;

public class MyCoursesService {
    private readonly AppDbContext _db;

    public MyCoursesService(AppDbContext db) {
        _db = db;
    }

    public async Task ShowAsync(int userId) {
        var enrollments = await _db.UserCourseEnrollments
            .AsNoTracking()
            .Where(e => e.UserId == userId)
            .Include(e => e.Course)
            .OrderBy(e => e.Course.Title)
            .ToListAsync();

        Console.Clear();
        Console.WriteLine("========== MY ENROLLED COURSES ==========\n");

        if(enrollments.Count == 0) {
            Console.WriteLine(
                "You are not enrolled in any courses yet.");
            return;
        }

        foreach(var enrollment in enrollments) {
            Console.WriteLine(
                $"[{enrollment.CourseId}] {enrollment.Course.Title}");

            if(!string.IsNullOrWhiteSpace(
                    enrollment.Course.Description)) {
                Console.WriteLine(
                    $"    {enrollment.Course.Description}");
            }

            Console.WriteLine("-----------------------------------------");
        }
    }
}
