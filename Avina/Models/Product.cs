namespace Avina.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Image { get; set; }
    public decimal Price { get; set; } = 0; // Fiat price
    public int CoinPrice { get; set; } = 0; // Coin price
    public string Category { get; set; } = string.Empty;
    public string Type { get; set; } = "Physical"; // Physical, Digital, Podcast, Pdf
    public string? FilePath { get; set; } // Path for digital content
    public int PreviewPages { get; set; } = 0; // Pages to show free (for PDF)
    public bool IsNew { get; set; } = true;
    public int Stock { get; set; } = 999;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    // ====== Path connection (per design doc: products are "tools to practice the path") ======
    /// <summary>
    /// مسیر رشد مرتبط (مثلا بردگیم حل مسئله ⇒ مسیر تحلیل‌گر).
    /// </summary>
    public int? RelatedPathId { get; set; }
    public virtual GrowthPath? RelatedPath { get; set; }

    /// <summary>
    /// مهارتی که این محصول تمرینش می‌کند (اختیاری).
    /// </summary>
    public int? RelatedSkillId { get; set; }
    public virtual Skill? RelatedSkill { get; set; }

    /// <summary>
    /// کد QR منحصربه‌فرد روی محصول. وقتی کاربر اسکن می‌کند، محتوای ویژه/ماموریت مرتبط باز می‌شود.
    /// خالی می‌تواند باشد (مثلاً برای محصول دیجیتال یا قبل از تولید).
    /// </summary>
    public string? QrCode { get; set; }

    /// <summary>
    /// Coin/XP اضافه که با اسکن کد QR بعد از خرید فعال می‌شود (یک بار).
    /// </summary>
    public int UnlockBonusCoin { get; set; } = 0;
    public int UnlockBonusXP { get; set; } = 0;

    // Navigation property
    public virtual ICollection<UserPurchase> UserPurchases { get; set; } = new List<UserPurchase>();
}

/// <summary>
/// لاگ اسکن کد QR محصولات: تضمین یک‌بار unlock، تحلیل رفتار خرید.
/// </summary>
public class ProductQrUnlock
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public string QrCode { get; set; } = string.Empty;
    public DateTime UnlockedAt { get; set; } = DateTime.UtcNow;
    public int GrantedCoin { get; set; }
    public int GrantedXP { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
}
