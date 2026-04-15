using Avina.Models;
using Avina.Services;
using Microsoft.AspNetCore.Mvc;

namespace Avina.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    // GET api/users/me
    [HttpGet("me")]
    public async Task<ActionResult<User>> GetCurrentUser()
    {
        var user = await _userService.GetCurrentUserAsync();
        if (user is null)
            return NotFound(new { message = "کاربری یافت نشد" });

        return Ok(user);
    }

    // GET api/users/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<User>> GetById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user is null)
            return NotFound(new { message = $"کاربر با شناسه {id} یافت نشد" });

        return Ok(user);
    }

    // PUT api/users/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] User user)
    {
        if (id != user.Id)
            return BadRequest(new { message = "شناسه کاربر مطابقت ندارد" });

        await _userService.UpdateUserAsync(user);
        return NoContent();
    }

    // POST api/users/login
    [HttpPost("login")]
    public async Task<ActionResult<object>> Login([FromBody] LoginRequest request)
    {
        var success = await _userService.LoginAsync(request.Email, request.Password);
        if (!success)
            return Unauthorized(new { message = "ایمیل یا رمز عبور اشتباه است" });

        return Ok(new { message = "ورود موفق" });
    }

    // POST api/users/logout
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _userService.LogoutAsync();
        return Ok(new { message = "خروج موفق" });
    }
}

public record LoginRequest(string Email, string Password);
