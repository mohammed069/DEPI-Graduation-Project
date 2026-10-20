
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.Models;

namespace QuizApp.Services;

public class CourseEnrollmentService {
    private readonly AppDbContext _db;

    public CourseEnrollmentService(AppDbContext db) {
        _db = db;
    }

    public async Task EnrollAsync(int userId) {
        var courses = await _db.Courses
            .AsNoTracking()
            .OrderBy(c => c.CourseId)
            .ToListAsync();

        Console.Clear();
        Console.WriteLine("========== ENROLL IN A COURSE ==========\n");

        if(courses.Count == 0) {
            Console.WriteLine("No courses available.");
            return;
        }

        foreach(var course in courses) {
            Console.WriteLine(
                $"[{course.CourseId}] {course.Title}");
        }

        Console.Write("\nEnter Course ID (0 to cancel): ");

        if(!int.TryParse(Console.ReadLine(), out int courseId) ||
            courseId < 0) {
            Console.WriteLine("Invalid Course ID.");
            return;
        }

        if(courseId == 0)
            return;

        var selectedCourse = await _db.Courses
            .FirstOrDefaultAsync(c => c.CourseId == courseId);

        if(selectedCourse == null) {
            Console.WriteLine("Course not found.");
            return;
        }

        bool alreadyEnrolled =
            await _db.UserCourseEnrollments.AnyAsync(e =>
                e.UserId == userId &&
                e.CourseId == courseId);

        if(alreadyEnrolled) {
            Console.WriteLine("You are already enrolled in this course.");
            return;
        }


        var lastEnrollmentId = await _db.UserCourseEnrollments
            .MaxAsync(e => (int?)e.EnrollmentId) ?? 0;

        var enrollment = new UserCourseEnrollment {
            EnrollmentId = lastEnrollmentId + 1,
            UserId = userId,
            CourseId = courseId
        };

        _db.UserCourseEnrollments.Add(enrollment);
        await _db.SaveChangesAsync();


        Console.WriteLine(
            $"\nSuccessfully enrolled in: {selectedCourse.Title}");
    }
}
