using Microsoft.EntityFrameworkCore;
using QuizApp;
using QuizApp.Data;
using QuizApp.Models;
using QuizApp.Services;
using System;
using System.Text;


var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlServer(
        "Server=MOHAMMED\\SQLEXPRESS;Database=QuizAppDB;Trusted_Connection=True;TrustServerCertificate=True;")
    .Options;

await using var db = new AppDbContext(options);


var loginService = new LoginService(db);

User? currentUser = null;

while(currentUser == null) {
    //Console.Clear();
    //Console.ForegroundColor = ConsoleColor.Cyan;
    //Console.WriteLine("==============================================");
    //Console.WriteLine("     ONLINE QUIZ & EXAM MANAGEMENT SYSTEM");
    //Console.WriteLine("                    LOGIN");
    //Console.WriteLine("==============================================");
    //Console.ResetColor();

    //Console.Write("\nUsername: ");
    //string username = Console.ReadLine()?.Trim() ?? "";

    //Console.Write("Password: ");
    //string password = ReadPassword();

    //currentUser = await loginService.LoginAsync(username, password);

    //if(currentUser == null) {
    //    Console.ForegroundColor = ConsoleColor.Red;
    //    Console.WriteLine("\nInvalid username or password.");
    //    Console.ResetColor();

    //    Console.WriteLine("Press any key to try again...");
    //    Console.ReadKey();
    //}

    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("==============================================");
    Console.WriteLine("     ONLINE QUIZ & EXAM MANAGEMENT SYSTEM");
    Console.WriteLine("==============================================");
    Console.ResetColor();

    Console.WriteLine("\n1. Login");
    Console.WriteLine("2. Sign Up");
    Console.WriteLine("0. Exit");
    Console.Write("\nChoose an option: ");

    string? choice = Console.ReadLine();

    switch(choice) {
        case "1": {
            Console.Write("\nUsername: ");
            string username = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Password: ");
            string password = ReadPassword();

            currentUser = await loginService.LoginAsync(
                username, password);

            if(currentUser == null) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid username or password.");
                Console.ResetColor();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }

            break;
        }

        case "2": {
            Console.Write("\nFull Name: ");
            string name = Console.ReadLine() ?? "";

            Console.Write("Email: ");
            string email = Console.ReadLine() ?? "";

            Console.Write("Username: ");
            string username = Console.ReadLine() ?? "";

            Console.Write("Password: ");
            string password = ReadPassword();

            Console.Write("\nConfirm Password: ");
            string confirmPassword = ReadPassword();

            if(password != confirmPassword) {
                Console.WriteLine("Passwords do not match.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                break;
            }

            var result = await loginService.SignUpAsync(
                name, email, username, password);

            Console.WriteLine($"\n{result.Message}");

            if(result.Success) {
                currentUser = result.NewUser;
                Console.WriteLine($"Welcome, {currentUser!.Name}!");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            break;
        }

        case "0":
            Console.WriteLine("Goodbye!");
            return;

        default:
            Console.WriteLine("Invalid choice.");
            Console.ReadKey();
            break;
    }
}

Console.Clear();
Console.WriteLine($"\nWelcome, {currentUser.Name}!");
Console.WriteLine("Press any key to continue...");
Console.ReadKey();



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
            break;

        case "2":
            Console.WriteLine("\nEnroll in a Course selected.");
            break;

        case "3":
            Console.WriteLine("\nView My Enrolled Courses selected.");
            break;

        case "4":
            await TakeQuiz.RunAsync(db, currentUser.UserId);
            break;

        case "5":
            Console.Clear();
            await AttemptServices.viewMyResultsAsync(
                db,
                currentUser.UserId);
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




static string ReadPassword() {
    string password = "";

    while(true) {
        var key = Console.ReadKey(intercept: false);

        if(key.Key == ConsoleKey.Enter) {
            Console.WriteLine();
            return password;
        }

        if(key.Key == ConsoleKey.Backspace) {
            if(password.Length > 0)
                password = password[..^1];

            continue;
        }

        if(!char.IsControl(key.KeyChar))
            password += key.KeyChar;
    }
}

