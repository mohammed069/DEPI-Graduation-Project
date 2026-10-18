using QuizApp;
using System;
using System.Text;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;


var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlServer(
        "Server=MOHAMMED\\SQLEXPRESS;Database=QuizAppDB;Trusted_Connection=True;TrustServerCertificate=True;")
    .Options;

await using var db = new AppDbContext(options);


Console.OutputEncoding = Encoding.UTF8;
Console.Title = "Online Quiz & Exam Management System";

bool isRunning = true;

while(isRunning) {
    Console.Clear();

    Console.ForegroundColor = ConsoleColor.Cyan;

    Console.WriteLine("==============================================");
    Console.WriteLine("     ONLINE QUIZ & EXAM MANAGEMENT SYSTEM");
    Console.WriteLine("==============================================");

    Console.ResetColor();

    Console.WriteLine();
    Console.WriteLine("  1. View All Courses");
    Console.WriteLine("  2. Enroll in a Course");
    Console.WriteLine("  3. View My Enrolled Courses");
    Console.WriteLine("  4. Take a Quiz");
    Console.WriteLine("  5. View My Results");
    Console.WriteLine("  0. Exit");

    Console.WriteLine();
    Console.Write("Enter your choice: ");

    string? choice = Console.ReadLine();

    switch(choice) {
        case "1":
            Console.Clear();
            await CoursesAndQuizzes.ShowAsync(db);

            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey();
            break;

        case "2":
            Console.WriteLine("\nEnroll in a Course selected.");
            break;

        case "3":
            Console.WriteLine("\nView My Enrolled Courses selected.");
            break;

        case "4":
            await TakeQuiz.RunAsync(db);
            break;



        case "5":
            Console.WriteLine("\nView My Results selected.");
            break;

        case "0":
            isRunning = false;
            Console.WriteLine("\nThank you for using our system!");
            break;

        default:
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nInvalid choice! Please try again.");
            Console.ResetColor();
            break;
    }

    if(isRunning) {
        Console.WriteLine("\nPress any key to return to the Main Menu...");
        Console.ReadKey();
    }
}
