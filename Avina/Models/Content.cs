namespace Avina.Models;

public enum ContentMediaType { Image, PDF, Audio, Video }

public class ContentMedia
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public ContentMediaType Type { get; set; }
    public string FilePath { get; set; } = "";
    public string? ThumbnailPath { get; set; }
    public bool IsFree { get; set; } = true;
    public int CoinPrice { get; set; } = 0;
    public string Category { get; set; } = "";
    public string Tags { get; set; } = "";
    public long FileSizeBytes { get; set; } = 0;
    public int DurationSeconds { get; set; } = 0; // for audio/video
    public int PageCount { get; set; } = 0;       // for PDF
    public int ViewCount { get; set; } = 0;
    public int PurchaseCount { get; set; } = 0;
    public int WeeklyViews { get; set; } = 0;
    public int MonthlyViews { get; set; } = 0;
    public int? UploaderId { get; set; }
    public bool IsPublished { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public User? Uploader { get; set; }
    public ICollection<ContentPurchase> Purchases { get; set; } = new List<ContentPurchase>();
}

public class ContentPurchase
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ContentId { get; set; }
    public int CoinSpent { get; set; }
    public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public ContentMedia Content { get; set; } = null!;
}
