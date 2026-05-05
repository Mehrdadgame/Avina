using Avina.Data;
using Avina.Models;
using Microsoft.EntityFrameworkCore;

namespace Avina.Services;

public interface IUserService
{
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetCurrentUserAsync();
    Task UpdateUserAsync(User user);
    Task<bool> LoginAsync(string email, string password);
    Task LogoutAsync();
}

public class UserService : IUserService
{
    private readonly AvinaDbContext _context;

    public UserService(AvinaDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);
    }

    public async Task<User?> GetCurrentUserAsync()
    {
        // در آینده از HttpContext/JWT claim می‌خوانیم — فعلاً اولین کاربر فعال
        return await _context.Users
            .FirstOrDefaultAsync(u => u.IsActive);
    }

    public async Task UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);

        if (user == null) return false;

        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }

    public Task LogoutAsync()
    {
        return Task.CompletedTask;
    }
}
