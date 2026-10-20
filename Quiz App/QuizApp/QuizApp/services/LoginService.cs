
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
}
