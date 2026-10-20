
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.Models;

namespace QuizApp.Services;

public class LoginService {
    private readonly AppDbContext _db;

    public LoginService(AppDbContext db) {
        _db = db;
    }

    public async Task<User?> LoginAsync(
        string username,
        string password) {
        var user = await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username);

        if(user == null || string.IsNullOrWhiteSpace(user.PasswordHash))
            return null;

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(
            password,
            user.PasswordHash);

        return isPasswordValid ? user : null;
    }


    public async Task<(bool Success, string Message, User? NewUser)>
        SignUpAsync(
            string name,
            string email,
            string username,
            string password) {
        name = name.Trim();
        email = email.Trim();
        username = username.Trim();
        password = password.Trim();

        if(string.IsNullOrWhiteSpace(name) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(password)) {
            return (false, "All fields are required.", null);
        }

        if(password.Length < 6) {
            return (false, "Password must be at least 6 characters.", null);
        }

        bool usernameExists = await _db.Users
            .AnyAsync(u => u.Username == username);

        if(usernameExists) {
            return (false, "Username already exists.", null);
        }

        bool emailExists = await _db.Users
            .AnyAsync(u => u.Email == email);

        if(emailExists) {
            return (false, "Email already exists.", null);
        }

        int newUserId = (await _db.Users
    .MaxAsync(u => (int?)u.UserId) ?? 0) + 1;

        var user = new User {
            UserId = newUserId,
            Name = name,
            Email = email,
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = "Student"
        };

        _db.Users.Add(user);

        try {
            await _db.SaveChangesAsync();
            return (true, "Account created successfully!", user);
        } catch(DbUpdateException) {
            _db.Entry(user).State = EntityState.Detached;
            return (false, "Could not create account. Check the entered data.", null);
        }
    }

}
