namespace Avina.Models;

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public decimal Price { get; set; }
    public int CoinPrice { get; set; }
    public string? Category { get; set; }
    public bool IsNew { get; set; }
    public int Stock { get; set; }
}
