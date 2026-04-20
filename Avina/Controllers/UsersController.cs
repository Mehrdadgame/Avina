using Avina.Data;
using Avina.Models;
using Avina.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Avina.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AvinaDbContext _context;
    private readonly ILogger<UsersController> _logger;

    public UsersController(AvinaDbContext context, ILogger<UsersController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET api/users/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserProfileDto>> GetById(int id)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
            return NotFound(new { message = $"کاربر با شناسه {id} یافت نشد" });

        return Ok(MapToProfileDto(user));
    }

    // GET api/users/email/{email}
    [HttpGet("email/{email}")]
    public async Task<ActionResult<UserProfileDto>> GetByEmail(string email)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user is null)
            return NotFound(new { message = "کاربر یافت نشد" });

        return Ok(MapToProfileDto(user));
    }

    // GET api/users/me (Authorized)
    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserProfileDto>> GetCurrentUser()
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            return Unauthorized();

        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
            return NotFound();

        return Ok(MapToProfileDto(user));
    }

    // PUT api/users/{id} (Authorized)
    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRequest request)
    {
        var user = await _context.Users.FindAsync(id);
        if (user is null)
            return NotFound();

        // Verify user is updating their own profile
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId) || currentUserId != id)
            return Forbid();

        // Update fields
        if (!string.IsNullOrWhiteSpace(request.Name))
            user.Name = request.Name.Trim();

        if (!string.IsNullOrWhiteSpace(request.Bio))
            user.Bio = request.Bio.Trim();

        if (!string.IsNullOrWhiteSpace(request.ProfileImage))
            user.ProfileImage = request.ProfileImage;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "پروفایل به‌روزرسانی شد", user = MapToProfileDto(user) });
    }

    // GET api/users/leaderboard
    [HttpGet("leaderboard")]
    [HttpGet("leaderboard/{top:int}")]
    public async Task<ActionResult<List<UserLeaderboardDto>>> GetLeaderboard(int top = 10)
    {
        var users = await _context.Users
            .AsNoTracking()
            .Where(u => u.IsActive)
            .OrderByDescending(u => u.Coin)
            .ThenByDescending(u => u.Level)
            .Take(top)
            .Select(u => new UserLeaderboardDto(
                u.Id,
                u.Name,
                u.Coin,
                u.Level,
                u.Experience,
                u.ProfileImage,
                u.Followers
            ))
            .ToListAsync();

        return Ok(users);
    }

    // GET api/users/{id}/coins
    [HttpGet("{id:int}/coins")]
    public async Task<ActionResult<CoinBalanceDto>> GetCoinBalance(int id)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
            return NotFound();

        return Ok(new CoinBalanceDto(user.Coin, user.Level, user.Experience));
    }

    // Private helper method
    private UserProfileDto MapToProfileDto(User user)
    {
        return new UserProfileDto(
            Id: user.Id,
            Name: user.Name,
            Email: user.Email,
            Role: user.Role,
            Coin: user.Coin,
            Level: user.Level,
            Experience: user.Experience,
            ProfileImage: user.ProfileImage,
            Bio: user.Bio,
            Followers: user.Followers,
            Following: user.Following,
            Achievements: user.Achievements,
            CreatedAt: user.CreatedAt
        );
    }
}

// DTOs
public record UpdateUserRequest(
    string? Name,
    string? Bio,
    string? ProfileImage
);

public record UserProfileDto(
    int Id,
    string Name,
    string Email,
    string Role,
    int Coin,
    int Level,
    int Experience,
    string? ProfileImage,
    string? Bio,
    int Followers,
    int Following,
    List<string> Achievements,
    DateTime CreatedAt
);

public record UserLeaderboardDto(
    int Id,
    string Name,
    int Coin,
    int Level,
    int Experience,
    string? ProfileImage,
    int Followers
);

public record CoinBalanceDto(
    int Coin,
    int Level,
    int Experience
);
