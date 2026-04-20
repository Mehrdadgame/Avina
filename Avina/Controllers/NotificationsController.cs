using Avina.Data;
using Avina.Models;
using Avina.Services;
using Microsoft.AspNetCore.Mvc;

namespace Avina.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;
    private readonly AvinaDbContext _db;

    public NotificationsController(INotificationService notificationService, AvinaDbContext db)
    {
        _notificationService = notificationService;
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetForUser(int userId, string? role = null, int take = 30)
    {
        if (userId <= 0)
        {
            return BadRequest(new { message = "شناسه کاربر معتبر نیست" });
        }

        var items = await _notificationService.GetUserNotificationsAsync(userId, role, Math.Clamp(take, 1, 100));
        return Ok(items);
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount(int userId, string? role = null)
    {
        if (userId <= 0)
        {
            return Ok(new { unread = 0 });
        }

        var unread = await _notificationService.GetUnreadCountAsync(userId, role);
        return Ok(new { unread });
    }

    [HttpPost("{notificationId:int}/read")]
    public async Task<IActionResult> MarkAsRead(int notificationId, [FromBody] NotificationActionRequest request)
    {
        if (request.UserId <= 0)
        {
            return BadRequest(new { message = "شناسه کاربر معتبر نیست" });
        }

        await _notificationService.MarkAsReadAsync(request.UserId, notificationId);
        return Ok(new { message = "اعلان خوانده شد" });
    }

    [HttpPost("read-all")]
    public async Task<IActionResult> MarkAllAsRead([FromBody] NotificationActionRequest request)
    {
        if (request.UserId <= 0)
        {
            return BadRequest(new { message = "شناسه کاربر معتبر نیست" });
        }

        await _notificationService.MarkAllAsReadAsync(request.UserId, request.Role);
        return Ok(new { message = "همه اعلان‌ها خوانده شد" });
    }

    // Lightweight admin endpoint for creating campaign/school notifications.
    [HttpPost("admin/create")]
    public async Task<IActionResult> Create([FromBody] CreateNotificationRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest(new { message = "عنوان و متن اعلان اجباری است" });
        }

        var entity = new Notification
        {
            Title = request.Title.Trim(),
            Message = request.Message.Trim(),
            Type = request.Type,
            LinkUrl = request.LinkUrl,
            LinkLabel = request.LinkLabel,
            Icon = request.Icon,
            IsBroadcast = request.IsBroadcast,
            TargetUserId = request.TargetUserId,
            TargetRole = request.TargetRole,
            IsActive = true,
            PublishAt = request.PublishAt ?? DateTime.UtcNow,
            ExpireAt = request.ExpireAt,
            CreatedAt = DateTime.UtcNow
        };

        _db.Notifications.Add(entity);
        await _db.SaveChangesAsync();

        return Ok(new { message = "اعلان با موفقیت ایجاد شد", id = entity.Id });
    }
}

public record NotificationActionRequest(int UserId, string? Role = null);

public record CreateNotificationRequest(
    string Title,
    string Message,
    NotificationType Type = NotificationType.General,
    string? LinkUrl = null,
    string? LinkLabel = null,
    string? Icon = null,
    bool IsBroadcast = true,
    int? TargetUserId = null,
    string? TargetRole = null,
    DateTime? PublishAt = null,
    DateTime? ExpireAt = null
);
