using Avina.Data;
using Avina.Models;
using Avina.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Avina.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContentController : ControllerBase
{
    private readonly AvinaDbContext _db;
    private readonly IWebHostEnvironment _env;

    public ContentController(AvinaDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    // GET /api/content?page=1&size=12&type=&category=&free=
    [HttpGet]
    public async Task<IActionResult> GetAll(int page = 1, int size = 12,
        string? type = null, string? category = null, bool? free = null)
    {
        var q = _db.Contents.Where(c => c.IsPublished).AsQueryable();
        if (!string.IsNullOrEmpty(type) && Enum.TryParse<ContentMediaType>(type, true, out var t)) q = q.Where(c => c.Type == t);
        if (!string.IsNullOrEmpty(category)) q = q.Where(c => c.Category == category);
        if (free.HasValue) q = q.Where(c => c.IsFree == free.Value);

        var total = await q.CountAsync();
        var items = await q.OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * size).Take(size)
            .Select(c => ToDto(c)).ToListAsync();

        return Ok(new { total, page, size, items });
    }

    // GET /api/content/home — sections for home page
    [HttpGet("home")]
    public async Task<IActionResult> GetHomeSections()
    {
        var now = DateTime.UtcNow;
        var weekAgo = now.AddDays(-7);
        var monthAgo = now.AddDays(-30);

        var latest = await _db.Contents.Where(c => c.IsPublished)
            .OrderByDescending(c => c.CreatedAt).Take(8)
            .Select(c => ToDto(c)).ToListAsync();

        var topWeek = await _db.Contents.Where(c => c.IsPublished)
            .OrderByDescending(c => c.WeeklyViews).Take(8)
            .Select(c => ToDto(c)).ToListAsync();

        var topMonth = await _db.Contents.Where(c => c.IsPublished)
            .OrderByDescending(c => c.MonthlyViews).Take(8)
            .Select(c => ToDto(c)).ToListAsync();

        var bestseller = await _db.Contents.Where(c => c.IsPublished && !c.IsFree)
            .OrderByDescending(c => c.PurchaseCount).Take(8)
            .Select(c => ToDto(c)).ToListAsync();

        var mostViewed = await _db.Contents.Where(c => c.IsPublished)
            .OrderByDescending(c => c.ViewCount).Take(8)
            .Select(c => ToDto(c)).ToListAsync();

        var featuredCourses = await _db.Courses.Where(c => c.IsPublished)
            .OrderByDescending(c => c.StudentCount).Take(6)
            .Select(c => new {
                c.Id, c.Title, c.Description, c.Category, c.Instructor,
                c.ThumbnailImage, c.DurationMinutes, c.CoinPrice, c.IsFree,
                c.StudentCount, c.Rating, c.RatingCount, c.Level
            }).ToListAsync();

        return Ok(new { latest, topWeek, topMonth, bestseller, mostViewed, featuredCourses });
    }

    // GET /api/content/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var c = await _db.Contents.Include(x => x.Uploader).FirstOrDefaultAsync(x => x.Id == id);
        if (c == null) return NotFound();
        c.ViewCount++;
        c.WeeklyViews++;
        c.MonthlyViews++;
        await _db.SaveChangesAsync();
        return Ok(ToDto(c));
    }

    // POST /api/content/{id}/purchase
    [HttpPost("{id}/purchase")]
    public async Task<IActionResult> Purchase(int id, [FromBody] PurchaseContentRequest req)
    {
        var content = await _db.Contents.FindAsync(id);
        if (content == null) return NotFound(new { message = "محتوا یافت نشد" });
        if (content.IsFree) return BadRequest(new { message = "این محتوا رایگان است" });

        var user = await _db.Users.FindAsync(req.UserId);
        if (user == null) return NotFound(new { message = "کاربر یافت نشد" });

        var alreadyPurchased = await _db.ContentPurchases
            .AnyAsync(p => p.UserId == req.UserId && p.ContentId == id);
        if (alreadyPurchased) return BadRequest(new { message = "قبلاً خریداری شده است" });

        if (user.Coin < content.CoinPrice)
            return BadRequest(new { message = $"کوین کافی نیست. نیاز: {content.CoinPrice}, موجودی: {user.Coin}" });

        user.Coin -= content.CoinPrice;
        content.PurchaseCount++;

        _db.ContentPurchases.Add(new ContentPurchase
        {
            UserId = req.UserId,
            ContentId = id,
            CoinSpent = content.CoinPrice
        });
        await _db.SaveChangesAsync();

        return Ok(new { message = "خرید موفق", newCoinBalance = user.Coin });
    }

    // GET /api/content/{id}/access?userId=
    [HttpGet("{id}/access")]
    public async Task<IActionResult> CheckAccess(int id, int? userId)
    {
        var content = await _db.Contents.FindAsync(id);
        if (content == null) return NotFound();

        if (content.IsFree) return Ok(new { hasAccess = true, isFree = true });
        if (!userId.HasValue) return Ok(new { hasAccess = false, isFree = false, coinPrice = content.CoinPrice });

        var purchased = await _db.ContentPurchases
            .AnyAsync(p => p.UserId == userId && p.ContentId == id);
        return Ok(new { hasAccess = purchased, isFree = false, coinPrice = content.CoinPrice });
    }

    // POST /api/content/upload (multipart form)
    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] UploadContentRequest req)
    {
        if (req.File == null || req.File.Length == 0)
            return BadRequest(new { message = "فایل انتخاب نشده" });

        var ext = Path.GetExtension(req.File.FileName).ToLower();
        var allowedExts = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".pdf", ".mp3", ".wav", ".mp4", ".mov", ".webm" };
        if (!allowedExts.Contains(ext))
            return BadRequest(new { message = "فرمت فایل مجاز نیست" });

        var type = ext switch
        {
            ".jpg" or ".jpeg" or ".png" or ".gif" or ".webp" => ContentMediaType.Image,
            ".pdf" => ContentMediaType.PDF,
            ".mp3" or ".wav" => ContentMediaType.Audio,
            _ => ContentMediaType.Video
        };

        var uploadsDir = Path.Combine(_env.WebRootPath, "uploads", type.ToString().ToLower() + "s");
        Directory.CreateDirectory(uploadsDir);
        var fileName = $"{Guid.NewGuid()}{ext}";
        var filePath = Path.Combine(uploadsDir, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
            await req.File.CopyToAsync(stream);

        var relPath = $"/uploads/{type.ToString().ToLower()}s/{fileName}";

        var content = new ContentMedia
        {
            Title = req.Title,
            Description = req.Description ?? "",
            Type = type,
            FilePath = relPath,
            Category = req.Category ?? "",
            Tags = req.Tags ?? "",
            IsFree = req.IsFree,
            CoinPrice = req.IsFree ? 0 : req.CoinPrice,
            FileSizeBytes = req.File.Length,
            UploaderId = req.UploaderId,
            IsPublished = true
        };

        _db.Contents.Add(content);
        await _db.SaveChangesAsync();

        return Ok(new { message = "آپلود موفق", id = content.Id, filePath = relPath });
    }

    private static object ToDto(ContentMedia c) => new
    {
        c.Id, c.Title, c.Description,
        type = c.Type.ToString(),
        c.FilePath, c.ThumbnailPath,
        c.IsFree, c.CoinPrice,
        c.Category, c.Tags,
        c.ViewCount, c.PurchaseCount,
        c.WeeklyViews, c.MonthlyViews,
        c.DurationSeconds, c.PageCount,
        c.FileSizeBytes,
        uploaderName = c.Uploader?.Name,
        c.CreatedAt
    };
}

public record PurchaseContentRequest(int UserId);
public record UploadContentRequest
{
    public string Title { get; init; } = "";
    public string? Description { get; init; }
    public string? Category { get; init; }
    public string? Tags { get; init; }
    public bool IsFree { get; init; } = true;
    public int CoinPrice { get; init; } = 0;
    public int? UploaderId { get; init; }
    public IFormFile? File { get; init; }
}
