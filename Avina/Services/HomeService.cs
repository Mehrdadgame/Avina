using Avina.Data;
using Avina.Models;
using Microsoft.EntityFrameworkCore;

namespace Avina.Services;

public class ContentDto
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Type { get; set; } = "";
    public string FilePath { get; set; } = "";
    public string? ThumbnailPath { get; set; }
    public bool IsFree { get; set; }
    public int CoinPrice { get; set; }
    public string Category { get; set; } = "";
    public string Tags { get; set; } = "";
    public int ViewCount { get; set; }
    public int PurchaseCount { get; set; }
    public int WeeklyViews { get; set; }
    public int MonthlyViews { get; set; }
    public int DurationSeconds { get; set; }
    public int PageCount { get; set; }
    public long FileSizeBytes { get; set; }
    public string? UploaderName { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class HomeBannerDto
{
    public string Badge { get; set; } = "";
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public string PrimaryLabel { get; set; } = "";
    public string PrimaryLink { get; set; } = "";
    public string SecondaryLabel { get; set; } = "";
    public string SecondaryLink { get; set; } = "";
    public string Stat1 { get; set; } = "";
    public string Stat1Label { get; set; } = "";
    public string Stat2 { get; set; } = "";
    public string Stat2Label { get; set; } = "";
}

public class FeaturedPickDto
{
    public string Type { get; set; } = "";
    public int EntityId { get; set; }
    public string Title { get; set; } = "";
    public string Subtitle { get; set; } = "";
    public string Badge { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public string LinkUrl { get; set; } = "";
}

public class HomeSectionsDto
{
    public List<ContentDto> Latest { get; set; } = new();
    public List<ContentDto> TopWeek { get; set; } = new();
    public List<ContentDto> TopMonth { get; set; } = new();
    public List<ContentDto> Bestseller { get; set; } = new();
    public List<ContentDto> MostViewed { get; set; } = new();
    public List<Course> FeaturedCourses { get; set; } = new();
    public List<HomeBannerDto> HeroBanners { get; set; } = new();
    public List<FeaturedPickDto> FeaturedPicks { get; set; } = new();
}

public interface IHomeService
{
    Task<HomeSectionsDto> GetHomeSectionsAsync();
}

public class HomeService : IHomeService
{
    private readonly AvinaDbContext _db;

    public HomeService(AvinaDbContext db)
    {
        _db = db;
    }

    public async Task<HomeSectionsDto> GetHomeSectionsAsync()
    {
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
            .ToListAsync();

        var heroBanners = await BuildHeroBannersAsync();
        var featuredPicks = await BuildFeaturedPicksAsync();

        return new HomeSectionsDto
        {
            Latest = latest,
            TopWeek = topWeek,
            TopMonth = topMonth,
            Bestseller = bestseller,
            MostViewed = mostViewed,
            FeaturedCourses = featuredCourses,
            HeroBanners = heroBanners,
            FeaturedPicks = featuredPicks
        };
    }

    private async Task<List<HomeBannerDto>> BuildHeroBannersAsync()
    {
        var now = DateTime.UtcNow;
        var rows = await _db.HomeBanners
            .Where(b => b.IsActive &&
                        (b.PublishAt == null || b.PublishAt <= now) &&
                        (b.ExpireAt == null || b.ExpireAt >= now))
            .OrderBy(b => b.DisplayOrder)
            .Take(8)
            .ToListAsync();

        var result = new List<HomeBannerDto>();
        foreach (var row in rows)
        {
            if (row.SelectionMode == HomeBannerSelectionMode.Manual)
            {
                result.Add(ToManualBanner(row));
            }
            else
            {
                var autoBanner = await ToAutoBannerAsync(row);
                if (autoBanner != null)
                {
                    result.Add(autoBanner);
                }
            }
        }

        return result;
    }

    private async Task<HomeBannerDto?> ToAutoBannerAsync(HomeBanner row)
    {
        if (row.AutoSourceType is null)
        {
            return null;
        }

        return row.AutoSourceType.Value switch
        {
            HomeBannerSourceType.Content => await BuildContentAutoBannerAsync(row),
            HomeBannerSourceType.Course => await BuildCourseAutoBannerAsync(row),
            HomeBannerSourceType.Product => await BuildProductAutoBannerAsync(row),
            _ => null
        };
    }

    private async Task<HomeBannerDto?> BuildContentAutoBannerAsync(HomeBanner row)
    {
        ContentMedia? content = null;
        if (row.AutoEntityId.HasValue)
        {
            content = await _db.Contents.FirstOrDefaultAsync(c => c.Id == row.AutoEntityId.Value && c.IsPublished);
        }
        else
        {
            content = row.AutoStrategy switch
            {
                HomeBannerAutoStrategy.TopWeek => await _db.Contents.Where(c => c.IsPublished).OrderByDescending(c => c.WeeklyViews).FirstOrDefaultAsync(),
                HomeBannerAutoStrategy.TopMonth => await _db.Contents.Where(c => c.IsPublished).OrderByDescending(c => c.MonthlyViews).FirstOrDefaultAsync(),
                HomeBannerAutoStrategy.Bestseller => await _db.Contents.Where(c => c.IsPublished && !c.IsFree).OrderByDescending(c => c.PurchaseCount).FirstOrDefaultAsync(),
                HomeBannerAutoStrategy.MostViewed => await _db.Contents.Where(c => c.IsPublished).OrderByDescending(c => c.ViewCount).FirstOrDefaultAsync(),
                _ => await _db.Contents.Where(c => c.IsPublished).OrderByDescending(c => c.CreatedAt).FirstOrDefaultAsync()
            };
        }

        if (content == null)
        {
            return null;
        }

        return new HomeBannerDto
        {
            Badge = string.IsNullOrWhiteSpace(row.Badge) ? "🔥 منتخب خودکار" : row.Badge,
            Title = content.Title,
            Description = string.IsNullOrWhiteSpace(content.Description) ? "محتوای منتخب بر اساس رفتار کاربران پیشنهاد شد." : content.Description,
            ImageUrl = !string.IsNullOrWhiteSpace(content.ThumbnailPath) ? content.ThumbnailPath! : GetImageForType("content"),
            PrimaryLabel = string.IsNullOrWhiteSpace(row.PrimaryLabel) ? "مشاهده محتوا" : row.PrimaryLabel,
            PrimaryLink = $"/content/{content.Id}",
            SecondaryLabel = string.IsNullOrWhiteSpace(row.SecondaryLabel) ? "رفتن به مدرسه" : row.SecondaryLabel!,
            SecondaryLink = string.IsNullOrWhiteSpace(row.SecondaryLink) ? "/school" : row.SecondaryLink!,
            Stat1 = content.ViewCount.ToString("N0"),
            Stat1Label = "بازدید",
            Stat2 = content.IsFree ? "رایگان" : $"{content.CoinPrice}",
            Stat2Label = content.IsFree ? "دسترسی" : "کوین"
        };
    }

    private async Task<HomeBannerDto?> BuildCourseAutoBannerAsync(HomeBanner row)
    {
        Course? course = null;
        if (row.AutoEntityId.HasValue)
        {
            course = await _db.Courses.FirstOrDefaultAsync(c => c.Id == row.AutoEntityId.Value && c.IsPublished);
        }
        else
        {
            course = await _db.Courses
                .Where(c => c.IsPublished)
                .OrderByDescending(c => c.StudentCount)
                .FirstOrDefaultAsync();
        }

        if (course == null)
        {
            return null;
        }

        return new HomeBannerDto
        {
            Badge = string.IsNullOrWhiteSpace(row.Badge) ? "🎓 منتخب آموزشی" : row.Badge,
            Title = course.Title,
            Description = string.IsNullOrWhiteSpace(course.Description) ? "دوره پرطرفدار هفته در دسترس تو قرار گرفت." : course.Description,
            ImageUrl = !string.IsNullOrWhiteSpace(course.ThumbnailImage) ? course.ThumbnailImage! : GetImageForType("course"),
            PrimaryLabel = string.IsNullOrWhiteSpace(row.PrimaryLabel) ? "ورود به دوره" : row.PrimaryLabel,
            PrimaryLink = $"/course/{course.Id}",
            SecondaryLabel = string.IsNullOrWhiteSpace(row.SecondaryLabel) ? "مشاهده مدرسه" : row.SecondaryLabel!,
            SecondaryLink = string.IsNullOrWhiteSpace(row.SecondaryLink) ? "/school" : row.SecondaryLink!,
            Stat1 = course.StudentCount.ToString("N0"),
            Stat1Label = "دانش‌آموز",
            Stat2 = course.Rating.ToString("F1"),
            Stat2Label = "امتیاز"
        };
    }

    private async Task<HomeBannerDto?> BuildProductAutoBannerAsync(HomeBanner row)
    {
        Product? product = null;
        if (row.AutoEntityId.HasValue)
        {
            product = await _db.Products.FirstOrDefaultAsync(p => p.Id == row.AutoEntityId.Value && p.IsActive);
        }
        else
        {
            product = await _db.Products
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.IsNew)
                .ThenByDescending(p => p.CreatedAt)
                .FirstOrDefaultAsync();
        }

        if (product == null)
        {
            return null;
        }

        return new HomeBannerDto
        {
            Badge = string.IsNullOrWhiteSpace(row.Badge) ? "🛍️ پیشنهاد فروشگاه" : row.Badge,
            Title = product.Name,
            Description = string.IsNullOrWhiteSpace(product.Description) ? "پیشنهاد ویژه فروشگاه برای مسیر رشد تو." : product.Description,
            ImageUrl = !string.IsNullOrWhiteSpace(product.Image) ? product.Image! : GetImageForType("product"),
            PrimaryLabel = string.IsNullOrWhiteSpace(row.PrimaryLabel) ? "مشاهده محصول" : row.PrimaryLabel,
            PrimaryLink = $"/product/{product.Id}",
            SecondaryLabel = string.IsNullOrWhiteSpace(row.SecondaryLabel) ? "رفتن به فروشگاه" : row.SecondaryLabel!,
            SecondaryLink = string.IsNullOrWhiteSpace(row.SecondaryLink) ? "/store" : row.SecondaryLink!,
            Stat1 = product.CoinPrice.ToString("N0"),
            Stat1Label = "کوین",
            Stat2 = product.IsNew ? "جدید" : product.Type,
            Stat2Label = "وضعیت"
        };
    }

    private static HomeBannerDto ToManualBanner(HomeBanner row)
    {
        return new HomeBannerDto
        {
            Badge = row.Badge,
            Title = row.Title,
            Description = row.Description,
            ImageUrl = string.IsNullOrWhiteSpace(row.ImageUrl) ? GetImageForType("manual") : row.ImageUrl,
            PrimaryLabel = row.PrimaryLabel,
            PrimaryLink = row.PrimaryLink,
            SecondaryLabel = row.SecondaryLabel ?? "جزئیات",
            SecondaryLink = row.SecondaryLink ?? "/",
            Stat1 = row.Stat1 ?? "",
            Stat1Label = row.Stat1Label ?? "",
            Stat2 = row.Stat2 ?? "",
            Stat2Label = row.Stat2Label ?? ""
        };
    }

    private async Task<List<FeaturedPickDto>> BuildFeaturedPicksAsync()
    {
        const int targetCount = 6;
        var now = DateTime.UtcNow;
        var list = new List<FeaturedPickDto>();
        var usedKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var manualRows = await _db.HomeFeaturedItems
            .Where(f => f.SectionKey == "home_curated" &&
                        f.IsActive &&
                        (f.PublishAt == null || f.PublishAt <= now) &&
                        (f.ExpireAt == null || f.ExpireAt >= now))
            .OrderBy(f => f.DisplayOrder)
            .ToListAsync();

        foreach (var row in manualRows)
        {
            var dto = await ResolveFeaturedItemAsync(row);
            if (dto == null)
            {
                continue;
            }

            var key = $"{dto.Type}:{dto.EntityId}";
            if (usedKeys.Add(key))
            {
                list.Add(dto);
            }

            if (list.Count >= targetCount)
            {
                return list;
            }
        }

        var autoContent = await _db.Contents
            .Where(c => c.IsPublished)
            .OrderByDescending(c => c.WeeklyViews)
            .ThenByDescending(c => c.ViewCount)
            .Take(6)
            .ToListAsync();
        foreach (var c in autoContent)
        {
            if (TryAddPick(list, usedKeys, new FeaturedPickDto
                {
                    Type = "Content",
                    EntityId = c.Id,
                    Title = c.Title,
                    Subtitle = c.Category,
                    Badge = "🔥 منتخب هوشمند",
                    ImageUrl = !string.IsNullOrWhiteSpace(c.ThumbnailPath) ? c.ThumbnailPath! : GetImageForType("content"),
                    LinkUrl = $"/content/{c.Id}"
                }, targetCount))
            {
                break;
            }
        }

        var autoCourses = await _db.Courses
            .Where(c => c.IsPublished)
            .OrderByDescending(c => c.StudentCount)
            .Take(4)
            .ToListAsync();
        foreach (var c in autoCourses)
        {
            if (TryAddPick(list, usedKeys, new FeaturedPickDto
                {
                    Type = "Course",
                    EntityId = c.Id,
                    Title = c.Title,
                    Subtitle = c.Instructor,
                    Badge = "🎓 کلاس پیشنهادی",
                    ImageUrl = !string.IsNullOrWhiteSpace(c.ThumbnailImage) ? c.ThumbnailImage! : GetImageForType("course"),
                    LinkUrl = $"/course/{c.Id}"
                }, targetCount))
            {
                break;
            }
        }

        var autoProducts = await _db.Products
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.IsNew)
            .ThenByDescending(p => p.CreatedAt)
            .Take(3)
            .ToListAsync();
        foreach (var p in autoProducts)
        {
            if (TryAddPick(list, usedKeys, new FeaturedPickDto
                {
                    Type = "Product",
                    EntityId = p.Id,
                    Title = p.Name,
                    Subtitle = p.Category,
                    Badge = "🛍️ انتخاب فروشگاه",
                    ImageUrl = !string.IsNullOrWhiteSpace(p.Image) ? p.Image! : GetImageForType("product"),
                    LinkUrl = $"/product/{p.Id}"
                }, targetCount))
            {
                break;
            }
        }

        return list;
    }

    private async Task<FeaturedPickDto?> ResolveFeaturedItemAsync(HomeFeaturedItem row)
    {
        return row.EntityType switch
        {
            FeaturedEntityType.Content => await ResolveContentPick(row),
            FeaturedEntityType.Course => await ResolveCoursePick(row),
            FeaturedEntityType.Product => await ResolveProductPick(row),
            _ => null
        };
    }

    private async Task<FeaturedPickDto?> ResolveContentPick(HomeFeaturedItem row)
    {
        var content = await _db.Contents.FirstOrDefaultAsync(c => c.Id == row.EntityId && c.IsPublished);
        if (content == null)
        {
            return null;
        }

        return new FeaturedPickDto
        {
            Type = "Content",
            EntityId = content.Id,
            Title = row.TitleOverride ?? content.Title,
            Subtitle = row.SubtitleOverride ?? content.Category,
            Badge = row.Badge ?? "📚 منتخب دستی",
            ImageUrl = row.ImageUrlOverride ?? content.ThumbnailPath ?? GetImageForType("content"),
            LinkUrl = $"/content/{content.Id}"
        };
    }

    private async Task<FeaturedPickDto?> ResolveCoursePick(HomeFeaturedItem row)
    {
        var course = await _db.Courses.FirstOrDefaultAsync(c => c.Id == row.EntityId && c.IsPublished);
        if (course == null)
        {
            return null;
        }

        return new FeaturedPickDto
        {
            Type = "Course",
            EntityId = course.Id,
            Title = row.TitleOverride ?? course.Title,
            Subtitle = row.SubtitleOverride ?? course.Instructor,
            Badge = row.Badge ?? "🎓 منتخب دستی",
            ImageUrl = row.ImageUrlOverride ?? course.ThumbnailImage ?? GetImageForType("course"),
            LinkUrl = $"/course/{course.Id}"
        };
    }

    private async Task<FeaturedPickDto?> ResolveProductPick(HomeFeaturedItem row)
    {
        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == row.EntityId && p.IsActive);
        if (product == null)
        {
            return null;
        }

        return new FeaturedPickDto
        {
            Type = "Product",
            EntityId = product.Id,
            Title = row.TitleOverride ?? product.Name,
            Subtitle = row.SubtitleOverride ?? product.Category,
            Badge = row.Badge ?? "🛍️ منتخب دستی",
            ImageUrl = row.ImageUrlOverride ?? product.Image ?? GetImageForType("product"),
            LinkUrl = $"/product/{product.Id}"
        };
    }

    private static bool TryAddPick(List<FeaturedPickDto> list, HashSet<string> usedKeys, FeaturedPickDto item, int targetCount)
    {
        if (list.Count >= targetCount)
        {
            return true;
        }

        var key = $"{item.Type}:{item.EntityId}";
        if (usedKeys.Add(key))
        {
            list.Add(item);
        }

        return list.Count >= targetCount;
    }

    private static string GetImageForType(string type) => type switch
    {
        "content" => "https://images.unsplash.com/photo-1516116216624-53e697900ea0?w=1200&q=80",
        "course" => "https://images.unsplash.com/photo-1434030216411-0b793f4b6f97?w=1200&q=80",
        "product" => "https://images.unsplash.com/photo-1522202176988-66273c2fd55f?w=1200&q=80",
        _ => "https://images.unsplash.com/photo-1503676260728-1c00da094a0b?w=1200&q=80"
    };

    private static ContentDto ToDto(ContentMedia c) => new()
    {
        Id = c.Id,
        Title = c.Title,
        Description = c.Description,
        Type = c.Type.ToString(),
        FilePath = c.FilePath,
        ThumbnailPath = c.ThumbnailPath,
        IsFree = c.IsFree,
        CoinPrice = c.CoinPrice,
        Category = c.Category,
        Tags = c.Tags,
        ViewCount = c.ViewCount,
        PurchaseCount = c.PurchaseCount,
        WeeklyViews = c.WeeklyViews,
        MonthlyViews = c.MonthlyViews,
        DurationSeconds = c.DurationSeconds,
        PageCount = c.PageCount,
        FileSizeBytes = c.FileSizeBytes,
        UploaderName = c.Uploader?.Name,
        CreatedAt = c.CreatedAt
    };
}
