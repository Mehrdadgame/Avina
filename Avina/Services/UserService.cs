using Avina.Models;

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
    public Task<User?> GetUserByIdAsync(int id)
    {
        // Mock implementation - بعداً از دیتابیس پر کنیم
        return Task.FromResult<User?>(new User
        {
            Id = id,
            Name = "علی محمدی",
            Email = "ali@example.com",
            Role = "دانش‌آموز",
            ProfileImage = "images/profiles/user1.jpg",
            Bio = "فعال و پرانرژی",
            Coin = 2450,
            Followers = 125,
            Following = 89,
            CreatedAt = DateTime.Now.AddMonths(-3)
        });
    }

    public Task<User?> GetCurrentUserAsync()
    {
        // Mock implementation
        return GetUserByIdAsync(1);
    }

    public Task UpdateUserAsync(User user)
    {
        // Mock implementation
        return Task.CompletedTask;
    }

    public Task<bool> LoginAsync(string email, string password)
    {
        // Mock implementation
        return Task.FromResult(true);
    }

    public Task LogoutAsync()
    {
        // Mock implementation
        return Task.CompletedTask;
    }
}
