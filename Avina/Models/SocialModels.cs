namespace Avina.Models;

public class SocialPost
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? VideoUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual ICollection<SocialComment> Comments { get; set; } = new List<SocialComment>();
    public virtual ICollection<SocialPostLike> Likes { get; set; } = new List<SocialPostLike>();
}

public class SocialComment
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual SocialPost Post { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}

/// <summary>
/// انواع reaction به سبک LinkedIn — کاربر فقط یک reaction روی هر پست می‌گذارد.
/// </summary>
public enum ReactionType
{
    Like = 1,        // 👍 می‌پسندم
    Love = 2,        // ❤️ دوست‌داشتنی
    Celebrate = 3,   // 👏 آفرین
    Insightful = 4,  // 💡 الهام‌بخش
    Curious = 5,     // 🤔 جالبه
    Funny = 6        // 😄 خنده‌دار
}

public class SocialPostLike
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    public ReactionType Reaction { get; set; } = ReactionType.Like;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual SocialPost Post { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}

public class UserFollow
{
    public int Id { get; set; }
    public int FollowerId { get; set; }
    public int FollowingId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual User Follower { get; set; } = null!;
    public virtual User Following { get; set; } = null!;
}
