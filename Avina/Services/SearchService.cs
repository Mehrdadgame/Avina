using Avina.Data;
using Microsoft.EntityFrameworkCore;

namespace Avina.Services;

public sealed class SearchResultItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public string Kind { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
}

public sealed class GlobalSearchResultDto
{
    public string Query { get; set; } = string.Empty;
    public List<SearchResultItemDto> Users { get; set; } = new();
    public List<SearchResultItemDto> Posts { get; set; } = new();
    public List<SearchResultItemDto> Courses { get; set; } = new();
    public List<SearchResultItemDto> Contents { get; set; } = new();
    public List<SearchResultItemDto> Products { get; set; } = new();
}

public interface ISearchService
{
    Task<GlobalSearchResultDto> SearchAsync(string query, CancellationToken cancellationToken = default);
}

public class SearchService(AvinaDbContext db) : ISearchService
{
    public async Task<GlobalSearchResultDto> SearchAsync(string query, CancellationToken cancellationToken = default)
    {
        var normalizedQuery = query.Trim();
        if (string.IsNullOrWhiteSpace(normalizedQuery))
        {
            return new GlobalSearchResultDto();
        }

        return new GlobalSearchResultDto
        {
            Query = normalizedQuery,
            Users = await db.Users
                .AsNoTracking()
                .Where(u => u.IsActive && (u.Name.Contains(normalizedQuery) || u.Email.Contains(normalizedQuery) || (u.Bio != null && u.Bio.Contains(normalizedQuery))))
                .OrderByDescending(u => u.Followers)
                .Take(8)
                .Select(u => new SearchResultItemDto
                {
                    Id = u.Id,
                    Title = u.Name,
                    Subtitle = u.Role,
                    Description = string.IsNullOrWhiteSpace(u.Bio) ? u.Email : u.Bio!,
                    Link = $"/social/profile/{u.Id}",
                    Kind = "کاربر",
                    ImageUrl = u.ProfileImage
                })
                .ToListAsync(cancellationToken),
            Posts = (await db.SocialPosts
                .AsNoTracking()
                .Include(p => p.User)
                .Where(p => p.Content.Contains(normalizedQuery) || (p.ImageUrl != null && p.ImageUrl.Contains(normalizedQuery)))
                .OrderByDescending(p => p.CreatedAt)
                .Take(8)
                .Select(p => new
                {
                    p.Id,
                    UserName = p.User.Name,
                    UserImage = p.User.ProfileImage,
                    p.Content
                })
                .ToListAsync(cancellationToken))
                .Select(p => new SearchResultItemDto
                {
                    Id = p.Id,
                    Title = p.UserName,
                    Subtitle = "پست اجتماعی",
                    Description = p.Content.Length > 140 ? p.Content.Substring(0, 140) + "..." : p.Content,
                    Link = "/social",
                    Kind = "پست",
                    ImageUrl = p.UserImage
                })
                .ToList(),
            Courses = await db.Courses
                .AsNoTracking()
                .Where(c => c.IsPublished && (c.Title.Contains(normalizedQuery) || c.Description.Contains(normalizedQuery) || c.Category.Contains(normalizedQuery)))
                .OrderByDescending(c => c.StudentCount)
                .Take(8)
                .Select(c => new SearchResultItemDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Subtitle = c.Instructor,
                    Description = c.Description,
                    Link = $"/course/{c.Id}",
                    Kind = "دوره",
                    ImageUrl = c.ThumbnailImage
                })
                .ToListAsync(cancellationToken),
            Contents = await db.Contents
                .AsNoTracking()
                .Where(c => c.IsPublished && (c.Title.Contains(normalizedQuery) || c.Description.Contains(normalizedQuery) || c.Category.Contains(normalizedQuery) || c.Tags.Contains(normalizedQuery)))
                .OrderByDescending(c => c.ViewCount)
                .Take(8)
                .Select(c => new SearchResultItemDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Subtitle = c.Category,
                    Description = c.Description,
                    Link = $"/content/{c.Id}",
                    Kind = "محتوا",
                    ImageUrl = c.ThumbnailPath
                })
                .ToListAsync(cancellationToken),
            Products = await db.Products
                .AsNoTracking()
                .Where(p => p.IsActive && (p.Name.Contains(normalizedQuery) || p.Description.Contains(normalizedQuery) || p.Category.Contains(normalizedQuery)))
                .OrderByDescending(p => p.IsNew)
                .ThenByDescending(p => p.CreatedAt)
                .Take(8)
                .Select(p => new SearchResultItemDto
                {
                    Id = p.Id,
                    Title = p.Name,
                    Subtitle = p.Category,
                    Description = p.Description,
                    Link = $"/product/{p.Id}",
                    Kind = "محصول",
                    ImageUrl = p.Image
                })
                .ToListAsync(cancellationToken)
        };
    }
}
