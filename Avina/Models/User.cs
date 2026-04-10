namespace Avina.Models;

public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? ProfileImage { get; set; }
    public string? Bio { get; set; }
    public string? Role { get; set; }
    public int Coin { get; set; }
    public int Followers { get; set; }
    public int Following { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<string> Achievements { get; set; } = new();
}
