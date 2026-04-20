namespace Avina.Models;

public class UserPurchase
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public int CoinSpent { get; set; }
    public decimal AmountSpent { get; set; }
    public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual User? User { get; set; }
    public virtual Product? Product { get; set; }
}
