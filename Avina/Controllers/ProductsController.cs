using Avina.Models;
using Avina.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Avina.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IPurchaseService _purchaseService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(
        IProductService productService,
        IPurchaseService purchaseService,
        ILogger<ProductsController> logger)
    {
        _productService = productService;
        _purchaseService = purchaseService;
        _logger = logger;
    }

    // GET api/products
    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetAllProducts()
    {
        var products = await _productService.GetAllProductsAsync();
        var dtos = products.Select(MapToDto).ToList();
        return Ok(dtos);
    }

    // GET api/products/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDetailDto>> GetProductById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
            return NotFound(new { message = "محصول یافت نشد" });

        return Ok(MapToDetailDto(product));
    }

    // GET api/products/type/{type}
    [HttpGet("type/{type}")]
    public async Task<ActionResult<List<ProductDto>>> GetProductsByType(string type)
    {
        var products = await _productService.GetProductsByTypeAsync(type);
        var dtos = products.Select(MapToDto).ToList();
        return Ok(dtos);
    }

    // GET api/products/category/{category}
    [HttpGet("category/{category}")]
    public async Task<ActionResult<List<ProductDto>>> GetProductsByCategory(string category)
    {
        var products = await _productService.GetProductsByCategoryAsync(category);
        var dtos = products.Select(MapToDto).ToList();
        return Ok(dtos);
    }

    // POST api/purchases
    [Authorize]
    [HttpPost("purchases")]
    public async Task<ActionResult> PurchaseProduct([FromBody] PurchaseRequest request)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            return Unauthorized();

        var (success, message) = await _purchaseService.PurchaseWithCoinsAsync(
            userId,
            request.ProductId,
            request.CoinAmount
        );

        if (!success)
            return BadRequest(new { message });

        var purchases = await _purchaseService.GetUserPurchasesAsync(userId);
        return Ok(new { message, purchases = purchases.Select(MapPurchaseToDto).ToList() });
    }

    // GET api/purchases/user/{userId}
    [Authorize]
    [HttpGet("purchases/user/{userId:int}")]
    public async Task<ActionResult<List<UserPurchaseDto>>> GetUserPurchases(int userId)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
            return Unauthorized();

        // Users can only see their own purchases
        if (currentUserId != userId)
            return Forbid();

        var purchases = await _purchaseService.GetUserPurchasesAsync(userId);
        var dtos = purchases.Select(MapPurchaseToDto).ToList();
        return Ok(dtos);
    }

    // GET api/purchases/{purchaseId}
    [Authorize]
    [HttpGet("purchases/{purchaseId:int}")]
    public async Task<ActionResult<UserPurchaseDto>> GetPurchaseDetails(int purchaseId)
    {
        var purchase = await _purchaseService.GetPurchaseDetailsAsync(purchaseId);
        if (purchase == null)
            return NotFound(new { message = "خرید یافت نشد" });

        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
            return Unauthorized();

        if (purchase.UserId != currentUserId)
            return Forbid();

        return Ok(MapPurchaseToDto(purchase));
    }

    // GET api/products/{productId}/has-purchased
    [Authorize]
    [HttpGet("{productId:int}/has-purchased")]
    public async Task<ActionResult> HasUserPurchased(int productId)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            return Unauthorized();

        var hasPurchased = await _purchaseService.HasUserPurchasedAsync(userId, productId);
        return Ok(new { hasPurchased });
    }

    // Helper methods
    private ProductDto MapToDto(Product product)
    {
        return new ProductDto(
            product.Id,
            product.Name,
            product.Description,
            product.Image,
            product.Price,
            product.CoinPrice,
            product.Category,
            product.Type,
            product.IsNew,
            product.Stock
        );
    }

    private ProductDetailDto MapToDetailDto(Product product)
    {
        return new ProductDetailDto(
            product.Id,
            product.Name,
            product.Description,
            product.Image,
            product.Price,
            product.CoinPrice,
            product.Category,
            product.Type,
            product.FilePath,
            product.PreviewPages,
            product.IsNew,
            product.Stock,
            product.CreatedAt
        );
    }

    private UserPurchaseDto MapPurchaseToDto(UserPurchase purchase)
    {
        return new UserPurchaseDto(
            purchase.Id,
            purchase.UserId,
            purchase.ProductId,
            purchase.Product?.Name ?? "Unknown",
            purchase.CoinSpent,
            purchase.AmountSpent,
            purchase.PurchasedAt
        );
    }
}

// DTOs
public record PurchaseRequest(
    int ProductId,
    int CoinAmount
);

public record ProductDto(
    int Id,
    string Name,
    string Description,
    string? Image,
    decimal Price,
    int CoinPrice,
    string Category,
    string Type,
    bool IsNew,
    int Stock
);

public record ProductDetailDto(
    int Id,
    string Name,
    string Description,
    string? Image,
    decimal Price,
    int CoinPrice,
    string Category,
    string Type,
    string? FilePath,
    int PreviewPages,
    bool IsNew,
    int Stock,
    DateTime CreatedAt
);

public record UserPurchaseDto(
    int Id,
    int UserId,
    int ProductId,
    string ProductName,
    int CoinSpent,
    decimal AmountSpent,
    DateTime PurchasedAt
);
