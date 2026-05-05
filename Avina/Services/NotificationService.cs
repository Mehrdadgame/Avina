using Avina.Data;
using Avina.Models;
using Microsoft.EntityFrameworkCore;

namespace Avina.Services;

public class UserNotificationDto
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Message { get; set; } = "";
    public NotificationType Type { get; set; }
    public string? LinkUrl { get; set; }
    public string? LinkLabel { get; set; }
    public string? Icon { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}

public interface INotificationService
{
    Task<int> GetUnreadCountAsync(int userId, string? role);
    Task<List<UserNotificationDto>> GetUserNotificationsAsync(int userId, string? role, int take = 30);
    Task MarkAsReadAsync(int userId, int notificationId);
    Task MarkAllAsReadAsync(int userId, string? role);
}

public class NotificationService : INotificationService
{
    private readonly AvinaDbContext _db;

    public NotificationService(AvinaDbContext db)
    {
        _db = db;
    }

    public async Task<int> GetUnreadCountAsync(int userId, string? role)
    {
        var now = DateTime.UtcNow;
        var query = from n in _db.Notifications
                    where n.IsActive
                          && (n.PublishAt == null || n.PublishAt <= now)
                          && (n.ExpireAt == null || n.ExpireAt >= now)
                          && ((n.IsBroadcast && IsRoleMatch(n.TargetRole, role)) || n.TargetUserId == userId)
                    join s in _db.UserNotificationStates.Where(x => x.UserId == userId)
                        on n.Id equals s.NotificationId into stateJoin
                    from s in stateJoin.DefaultIfEmpty()
                    where s == null || !s.IsRead
                    select n.Id;

        return await query.CountAsync();
    }

    public async Task<List<UserNotificationDto>> GetUserNotificationsAsync(int userId, string? role, int take = 30)
    {
        var now = DateTime.UtcNow;

        var notifications = await _db.Notifications
            .Where(n =>
                n.IsActive &&
                (n.PublishAt == null || n.PublishAt <= now) &&
                (n.ExpireAt == null || n.ExpireAt >= now) &&
                (
                    n.TargetUserId == userId ||
                    (n.IsBroadcast && (
                        n.TargetRole == null ||       
                        n.TargetRole == role          
                    ))
                ))
            .OrderByDescending(n => n.CreatedAt)
            .Take(take)
            .ToListAsync();

        var ids = notifications.Select(n => n.Id).ToList();
        var states = await _db.UserNotificationStates
            .Where(s => s.UserId == userId && ids.Contains(s.NotificationId))
            .ToDictionaryAsync(s => s.NotificationId, s => s);

        return notifications.Select(n =>
        {
            states.TryGetValue(n.Id, out var state);
            return new UserNotificationDto
            {
                Id = n.Id,
                Title = n.Title,
                Message = n.Message,
                Type = n.Type,
                LinkUrl = n.LinkUrl,
                LinkLabel = n.LinkLabel,
                Icon = n.Icon,
                IsRead = state?.IsRead ?? false,
                CreatedAt = n.CreatedAt
            };
        }).ToList();
    }

    public async Task MarkAsReadAsync(int userId, int notificationId)
    {
        var state = await _db.UserNotificationStates
            .FirstOrDefaultAsync(s => s.UserId == userId && s.NotificationId == notificationId);

        if (state == null)
        {
            _db.UserNotificationStates.Add(new UserNotificationState
            {
                UserId = userId,
                NotificationId = notificationId,
                IsRead = true,
                ReadAt = DateTime.UtcNow
            });
        }
        else if (!state.IsRead)
        {
            state.IsRead = true;
            state.ReadAt = DateTime.UtcNow;
        }

        await _db.SaveChangesAsync();
    }

    public async Task MarkAllAsReadAsync(int userId, string? role)
    {
        var now = DateTime.UtcNow;
        var notificationIds = await _db.Notifications
            .Where(n =>
                n.IsActive &&
                (n.PublishAt == null || n.PublishAt <= now) &&
                (n.ExpireAt == null || n.ExpireAt >= now) &&
                ((n.IsBroadcast && IsRoleMatch(n.TargetRole, role)) || n.TargetUserId == userId))
            .Select(n => n.Id)
            .ToListAsync();

        if (notificationIds.Count == 0)
        {
            return;
        }

        var existingStates = await _db.UserNotificationStates
            .Where(s => s.UserId == userId && notificationIds.Contains(s.NotificationId))
            .ToDictionaryAsync(s => s.NotificationId, s => s);

        foreach (var notificationId in notificationIds)
        {
            if (existingStates.TryGetValue(notificationId, out var state))
            {
                if (!state.IsRead)
                {
                    state.IsRead = true;
                    state.ReadAt = DateTime.UtcNow;
                }
            }
            else
            {
                _db.UserNotificationStates.Add(new UserNotificationState
                {
                    UserId = userId,
                    NotificationId = notificationId,
                    IsRead = true,
                    ReadAt = DateTime.UtcNow
                });
            }
        }

        await _db.SaveChangesAsync();
    }

    private static bool IsRoleMatch(string? notificationRole, string? userRole)
    {
        if (string.IsNullOrWhiteSpace(notificationRole))
        {
            return true;
        }

        if (string.IsNullOrWhiteSpace(userRole))
        {
            return false;
        }

        return Normalize(notificationRole) == Normalize(userRole);
    }

    private static string Normalize(string value)
        => value.Trim().Replace("‌", string.Empty).Replace(" ", string.Empty).ToLowerInvariant();
}
