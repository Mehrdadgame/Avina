using Avina.Data;
using Avina.Models;
using Microsoft.EntityFrameworkCore;

namespace Avina.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(string name, string email, string password);
    Task<AuthResponse> LoginAsync(string email, string password);
}

public class AuthService : IAuthService
{
    private readonly AvinaDbContext _context;
    private readonly JwtTokenService _jwtService;
    private readonly ILogger<AuthService> _logger;
    private readonly IConfiguration _configuration;

    public AuthService(
        AvinaDbContext context,
        JwtTokenService jwtService,
        ILogger<AuthService> logger,
        IConfiguration configuration)
    {
        _context = context;
        _jwtService = jwtService;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<AuthResponse> RegisterAsync(string name, string email, string password)
    {
        try
        {
            _logger.LogInformation($"[AuthService] Register attempt: {email}");

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || password.Length < 6)
            {
                return new AuthResponse(
                    Success: false,
                    Message: "نام، ایمیل و رمز عبور (حداقل 6 کاراکتر) ضروری است"
                );
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (existingUser != null)
            {
                return new AuthResponse(
                    Success: false,
                    Message: "این ایمیل قبلاً ثبت شده است"
                );
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);

            var user = new User
            {
                Name = name.Trim(),
                Email = email.Trim(),
                PasswordHash = passwordHash,
                PasswordSalt = "",
                Role = "دانش‌آموز",
                Coin = 100,
                Level = 1,
                ProfileImage = $"https://i.pravatar.cc/150?u={email}",
                CreatedAt = DateTime.UtcNow,
                LastLoginAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            var refreshDays = int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"] ?? "7");
            var refreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshDays),
                IsRevoked = false,
                CreatedAt = DateTime.UtcNow
            };
            _context.RefreshTokens.Add(refreshTokenEntity);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"[AuthService] User registered successfully: {user.Email}, Id={user.Id}");

            return new AuthResponse(
                Success: true,
                Message: "ثبت نام موفق",
                AccessToken: accessToken,
                RefreshToken: refreshToken,
                User: new UserAuthDto(
                    Id: user.Id,
                    Name: user.Name,
                    Email: user.Email,
                    Role: user.Role,
                    Coin: user.Coin,
                    Level: user.Level,
                    ProfileImage: user.ProfileImage
                )
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[AuthService] Register failed for {email}");
            return new AuthResponse(
                Success: false,
                Message: $"خطا در ثبت نام: {ex.Message}"
            );
        }
    }

    public async Task<AuthResponse> LoginAsync(string email, string password)
    {
        try
        {
            _logger.LogInformation($"[AuthService] Login attempt: {email}");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return new AuthResponse(
                    Success: false,
                    Message: "ایمیل یا رمز عبور اشتباه است"
                );
            }

            if (!user.IsActive)
            {
                return new AuthResponse(
                    Success: false,
                    Message: "حساب کاربری شما غیرفعال است"
                );
            }

            user.LastLoginAt = DateTime.UtcNow;
            _context.Users.Update(user);

            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            var refreshDays = int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"] ?? "7");
            var refreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshDays),
                IsRevoked = false,
                CreatedAt = DateTime.UtcNow
            };
            _context.RefreshTokens.Add(refreshTokenEntity);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"[AuthService] Login successful: {user.Email}");

            return new AuthResponse(
                Success: true,
                Message: "ورود موفق",
                AccessToken: accessToken,
                RefreshToken: refreshToken,
                User: new UserAuthDto(
                    Id: user.Id,
                    Name: user.Name,
                    Email: user.Email,
                    Role: user.Role,
                    Coin: user.Coin,
                    Level: user.Level,
                    ProfileImage: user.ProfileImage
                )
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[AuthService] Login failed for {email}");
            return new AuthResponse(
                Success: false,
                Message: $"خطا در ورود: {ex.Message}"
            );
        }
    }
}
