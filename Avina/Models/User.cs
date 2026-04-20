namespace Avina.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string PasswordSalt { get; set; } = string.Empty;
    public string? ProfileImage { get; set; }
    public string? Bio { get; set; }
    public string Role { get; set; } = "دانش‌آموز";
    public int Coin { get; set; } = 100;
    public int Level { get; set; } = 1;
    public int Experience { get; set; } = 0;
    public int Followers { get; set; } = 0;
    public int Following { get; set; } = 0;
    public List<string> Achievements { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastLoginAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<UserPurchase> Purchases { get; set; } = new List<UserPurchase>();
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
