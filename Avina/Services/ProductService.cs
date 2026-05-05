using Avina.Data;
using Avina.Models;
using Microsoft.EntityFrameworkCore;

namespace Avina.Services;

public interface IProductService
{
    Task<List<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task<List<Product>> GetProductsByTypeAsync(string type);
    Task<List<Product>> GetProductsByCategoryAsync(string category);
    Task<bool> UpdateProductAsync(int id, Product product);
}

public class ProductService : IProductService
{
    private readonly AvinaDbContext _context;
    private readonly ILogger<ProductService> _logger;

    public ProductService(AvinaDbContext context, ILogger<ProductService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _context.Products
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.IsNew)
            .ThenByDescending(p => p.CreatedAt)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Product>> GetProductsByTypeAsync(string type)
    {
        return await _context.Products
            .Where(p => p.IsActive && p.Type == type)
            .OrderByDescending(p => p.CreatedAt)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Product>> GetProductsByCategoryAsync(string category)
    {
        return await _context.Products
            .Where(p => p.IsActive && p.Category == category)
            .OrderByDescending(p => p.CreatedAt)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> UpdateProductAsync(int id, Product product)
    {
        var existing = await _context.Products.FindAsync(id);
        if (existing == null)
            return false;

        existing.Name = product.Name ?? existing.Name;
        existing.Description = product.Description ?? existing.Description;
        existing.Price = product.Price > 0 ? product.Price : existing.Price;
        existing.CoinPrice = product.CoinPrice > 0 ? product.CoinPrice : existing.CoinPrice;
        existing.Stock = product.Stock > 0 ? product.Stock : existing.Stock;
        existing.IsActive = product.IsActive;

        await _context.SaveChangesAsync();
        return true;
    }
}
