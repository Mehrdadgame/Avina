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

    // Navigation property
    public virtual ICollection<UserPurchase> UserPurchases { get; set; } = new List<UserPurchase>();
}
