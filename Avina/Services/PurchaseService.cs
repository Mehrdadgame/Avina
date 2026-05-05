using Avina.Data;
using Avina.Models;
using Microsoft.EntityFrameworkCore;

namespace Avina.Services;

public interface IPurchaseService
{
    Task<(bool Success, string Message)> PurchaseWithCoinsAsync(int userId, int productId, int coinAmount);
    Task<List<UserPurchase>> GetUserPurchasesAsync(int userId);
    Task<bool> HasUserPurchasedAsync(int userId, int productId);
    Task<UserPurchase?> GetPurchaseDetailsAsync(int purchaseId);
}

public class PurchaseService : IPurchaseService
{
    private readonly AvinaDbContext _context;
    private readonly ILogger<PurchaseService> _logger;

    public PurchaseService(AvinaDbContext context, ILogger<PurchaseService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<(bool Success, string Message)> PurchaseWithCoinsAsync(int userId, int productId, int coinAmount)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // Get user
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return (false, "کاربر یافت نشد");

            // Check if already purchased
            var alreadyPurchased = await _context.UserPurchases
                .AnyAsync(up => up.UserId == userId && up.ProductId == productId);

            if (alreadyPurchased)
                return (false, "شما قبلاً این محصول را خریداری کرده‌اید");

            // Get product
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return (false, "محصول یافت نشد");

            // Check coin balance
            if (user.Coin < coinAmount)
                return (false, $"سکه‌های شما ناکافی هستند. نیاز دارید: {coinAmount}, شما دارید: {user.Coin}");

            // Check stock for physical products
            if (product.Type == "Physical" && product.Stock <= 0)
                return (false, "محصول موجود نیست");

            // Deduct coins
            user.Coin -= coinAmount;
            user.Experience += 10; // Reward points for purchase

            // Reduce stock for physical products
            if (product.Type == "Physical")
                product.Stock--;

            // Create purchase record
            var purchase = new UserPurchase
            {
                UserId = userId,
                ProductId = productId,
                CoinSpent = coinAmount,
                AmountSpent = 0,
                PurchasedAt = DateTime.UtcNow
            };

            _context.Users.Update(user);
            _context.Products.Update(product);
            _context.UserPurchases.Add(purchase);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation($"User {userId} purchased product {productId} for {coinAmount} coins");
            return (true, "خریداری موفق! 🎉");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError($"Purchase failed: {ex.Message}");
            return (false, "خریداری ناموفق بود. دوباره تلاش کنید");
        }
    }

    public async Task<List<UserPurchase>> GetUserPurchasesAsync(int userId)
    {
        return await _context.UserPurchases
            .Where(up => up.UserId == userId)
            .Include(up => up.Product)
            .OrderByDescending(up => up.PurchasedAt)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> HasUserPurchasedAsync(int userId, int productId)
    {
        return await _context.UserPurchases
            .AnyAsync(up => up.UserId == userId && up.ProductId == productId);
    }

    public async Task<UserPurchase?> GetPurchaseDetailsAsync(int purchaseId)
    {
        return await _context.UserPurchases
            .Include(up => up.User)
            .Include(up => up.Product)
            .AsNoTracking()
            .FirstOrDefaultAsync(up => up.Id == purchaseId);
    }
}
