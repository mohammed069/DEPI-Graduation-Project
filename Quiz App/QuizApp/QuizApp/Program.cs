using Microsoft.EntityFrameworkCore;
using QuizApp.Data;


// add your connection string here
var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlServer(
        "Server=MOHAMMED\\SQLEXPRESS;Database=QuizAppDB;Trusted_Connection=True;TrustServerCertificate=True;")
    .Options;

await using var db = new AppDbContext(options);

try {
    bool connected = await db.Database.CanConnectAsync();

    Console.WriteLine(
        connected
            ? "Database connection successful!"
            : "Database connection failed.");
} catch(Exception ex) {
    Console.WriteLine(ex.Message);
}