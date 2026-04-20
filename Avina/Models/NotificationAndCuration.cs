namespace Avina.Models;

public enum NotificationType
{
    General = 0,
    Discount = 1,
    Store = 2,
    School = 3,
    System = 4
}

public class Notification
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Message { get; set; } = "";
    public NotificationType Type { get; set; } = NotificationType.General;
    public string? LinkUrl { get; set; }
    public string? LinkLabel { get; set; }
    public string? Icon { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsBroadcast { get; set; } = true;
    public int? TargetUserId { get; set; }
    public string? TargetRole { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PublishAt { get; set; }
    public DateTime? ExpireAt { get; set; }

    public User? TargetUser { get; set; }
    public ICollection<UserNotificationState> UserStates { get; set; } = new List<UserNotificationState>();
}

public class UserNotificationState
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int NotificationId { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public Notification Notification { get; set; } = null!;
}

public enum HomeBannerSelectionMode
{
    Manual = 0,
    Auto = 1
}

public enum HomeBannerSourceType
{
    Content = 1,
    Course = 2,
    Product = 3
}

public enum HomeBannerAutoStrategy
{
    Latest = 0,
    TopWeek = 1,
    TopMonth = 2,
    Bestseller = 3,
    MostViewed = 4
}

public class HomeBanner
{
    public int Id { get; set; }
    public string Badge { get; set; } = "";
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public string PrimaryLabel { get; set; } = "";
    public string PrimaryLink { get; set; } = "";
    public string? SecondaryLabel { get; set; }
    public string? SecondaryLink { get; set; }
    public string? Stat1 { get; set; }
    public string? Stat1Label { get; set; }
    public string? Stat2 { get; set; }
    public string? Stat2Label { get; set; }
    public int DisplayOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public DateTime? PublishAt { get; set; }
    public DateTime? ExpireAt { get; set; }
    public HomeBannerSelectionMode SelectionMode { get; set; } = HomeBannerSelectionMode.Manual;
    public HomeBannerSourceType? AutoSourceType { get; set; }
    public HomeBannerAutoStrategy? AutoStrategy { get; set; }
    public int? AutoEntityId { get; set; }
}

public enum FeaturedEntityType
{
    Content = 1,
    Course = 2,
    Product = 3
}

public class HomeFeaturedItem
{
    public int Id { get; set; }
    public string SectionKey { get; set; } = "home_curated";
    public FeaturedEntityType EntityType { get; set; } = FeaturedEntityType.Content;
    public int EntityId { get; set; }
    public string? TitleOverride { get; set; }
    public string? SubtitleOverride { get; set; }
    public string? Badge { get; set; }
    public string? ImageUrlOverride { get; set; }
    public int DisplayOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public DateTime? PublishAt { get; set; }
    public DateTime? ExpireAt { get; set; }
}
