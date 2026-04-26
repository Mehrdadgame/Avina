using Avina.Data;
using Avina.Models;
using Avina.Services;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Avina.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AvinaDbContext _context;
    private readonly JwtTokenService _jwtService;
    private readonly ILogger<AuthController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IProfileAvatarService _profileAvatarService;

    public AuthController(AvinaDbContext context, JwtTokenService jwtService, ILogger<AuthController> logger, IConfiguration configuration, IProfileAvatarService profileAvatarService)
    {
        _context = context;
        _jwtService = jwtService;
        _logger = logger;
        _configuration = configuration;
        _profileAvatarService = profileAvatarService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Email) || request.Password.Length < 6)
        {
            return BadRequest(new AuthResponse(
                Success: false,
                Message: "نام، ایمیل و رمز عبور (حداقل 6 کاراکتر) ضروری است"
            ));
        }

        // Check if user exists
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (existingUser != null)
        {
            return BadRequest(new AuthResponse(
                Success: false,
                Message: "این ایمیل قبلاً ثبت شده است"
            ));
        }

        // Hash password
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, workFactor: 12);
        var normalizedRole = _profileAvatarService.NormalizeRole("دانش‌آموز");

        // Create new user
        var user = new User
        {
            Name = request.Name.Trim(),
            FullName = request.Name.Trim(),
            Email = request.Email.Trim(),
            PasswordHash = passwordHash,
            PasswordSalt = "", // Not needed with bcrypt
            Role = normalizedRole,
            Coin = 100, // Welcome bonus
            Level = 1,
            ProfileImage = _profileAvatarService.GetDefaultAvatar(normalizedRole),
            AvatarUrl = _profileAvatarService.GetDefaultAvatar(normalizedRole),
            CreatedAt = DateTime.UtcNow,
            LastLoginAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Generate tokens
        var accessToken = _jwtService.GenerateAccessToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        // Save refresh token
        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"]!)),
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.RefreshTokens.Add(refreshTokenEntity);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"User registered successfully: {user.Email}");

        return Ok(new AuthResponse(
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
        ));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        // Find user
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return Unauthorized(new AuthResponse(
                Success: false,
                Message: "ایمیل یا رمز عبور اشتباه است"
            ));
        }

        // Check if user is active
        if (!user.IsActive)
        {
            return Unauthorized(new AuthResponse(
                Success: false,
                Message: "حساب کاربری شما غیرفعال است"
            ));
        }

        // Update last login
        user.LastLoginAt = DateTime.UtcNow;
        _context.Users.Update(user);

        // Generate tokens
        var accessToken = _jwtService.GenerateAccessToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        // Save refresh token
        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"]!)),
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.RefreshTokens.Add(refreshTokenEntity);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"User logged in: {user.Email}");

        return Ok(new AuthResponse(
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
        ));
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponse>> Refresh([FromBody] RefreshTokenRequest request)
    {
        var refreshTokenEntity = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken && !rt.IsRevoked);

        if (refreshTokenEntity == null || refreshTokenEntity.ExpiresAt < DateTime.UtcNow)
        {
            return Unauthorized(new AuthResponse(
                Success: false,
                Message: "Refresh token نامعتبر یا منقضی است"
            ));
        }

        var user = await _context.Users.FindAsync(refreshTokenEntity.UserId);
        if (user == null)
        {
            return Unauthorized(new AuthResponse(
                Success: false,
                Message: "کاربر یافت نشد"
            ));
        }

        // Generate new access token
        var newAccessToken = _jwtService.GenerateAccessToken(user);

        return Ok(new AuthResponse(
            Success: true,
            Message: "توکن تازه‌شده",
            AccessToken: newAccessToken,
            RefreshToken: request.RefreshToken, // Return same refresh token
            User: new UserAuthDto(
                Id: user.Id,
                Name: user.Name,
                Email: user.Email,
                Role: user.Role,
                Coin: user.Coin,
                Level: user.Level,
                ProfileImage: user.ProfileImage
            )
        ));
    }

    [HttpPost("logout")]
    public async Task<ActionResult<AuthResponse>> Logout([FromBody] RefreshTokenRequest request)
    {
        var refreshTokenEntity = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken);

        if (refreshTokenEntity != null)
        {
            refreshTokenEntity.IsRevoked = true;
            refreshTokenEntity.RevokedAt = DateTime.UtcNow;
            _context.RefreshTokens.Update(refreshTokenEntity);
            await _context.SaveChangesAsync();
        }

        return Ok(new AuthResponse(
            Success: true,
            Message: "خروج موفق"
        ));
    }

    [HttpGet("me")]
    public async Task<ActionResult<UserAuthDto>> GetCurrentUser()
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
        {
            return Unauthorized();
        }

        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(new UserAuthDto(
            Id: user.Id,
            Name: user.Name,
            Email: user.Email,
            Role: user.Role,
            Coin: user.Coin,
            Level: user.Level,
            ProfileImage: user.ProfileImage
        ));
    }
}
