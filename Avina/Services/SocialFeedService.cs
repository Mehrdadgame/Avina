using Avina.Data;
using Avina.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Avina.Services;

public sealed class SocialFeedPostDto
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string AuthorRole { get; set; } = string.Empty;
    public string? AuthorImage { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? VideoUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
    public bool IsLikedByCurrentUser { get; set; }
    public List<SocialFeedCommentDto> Comments { get; set; } = new();
}

public sealed class SocialFeedCommentDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserRole { get; set; } = string.Empty;
    public string? UserImage { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public sealed class SocialSuggestedUserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? ProfileImage { get; set; }
    public int Followers { get; set; }
    public bool IsFollowedByCurrentUser { get; set; }
}

public sealed class SocialFeedResultDto
{
    public List<SocialFeedPostDto> Posts { get; set; } = new();
    public List<SocialSuggestedUserDto> SuggestedUsers { get; set; } = new();
}

public sealed class SocialProfileDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? ProfileImage { get; set; }
    public string? Bio { get; set; }
    public int Followers { get; set; }
    public int Following { get; set; }
    public int PostCount { get; set; }
    public bool IsFollowedByCurrentUser { get; set; }
    public List<SocialFeedPostDto> Posts { get; set; } = new();
}

public interface ISocialFeedService
{
    Task<SocialFeedResultDto> GetFeedAsync(int? currentUserId = null, int? authorId = null, CancellationToken cancellationToken = default);
    Task<SocialFeedPostDto> CreatePostAsync(int userId, string content, string? imageUrl = null, string? videoUrl = null, CancellationToken cancellationToken = default);
    Task<SocialFeedCommentDto> AddCommentAsync(int userId, int postId, string content, CancellationToken cancellationToken = default);
    Task<bool> ToggleLikeAsync(int userId, int postId, CancellationToken cancellationToken = default);
    Task<SocialProfileDto?> GetProfileAsync(int profileUserId, int? currentUserId = null, CancellationToken cancellationToken = default);
    Task<bool> ToggleFollowAsync(int followerId, int followingId, CancellationToken cancellationToken = default);
}

public sealed class SocialFeedService(
    IDbContextFactory<AvinaDbContext> dbFactory,
    IProfileAvatarService profileAvatarService) : ISocialFeedService
{
    public async Task<SocialFeedResultDto> GetFeedAsync(int? currentUserId = null, int? authorId = null, CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        var query = db.SocialPosts
            .AsNoTracking()
            .Include(p => p.User)
            .Include(p => p.Likes)
            .Include(p => p.Comments)
                .ThenInclude(c => c.User)
            .OrderByDescending(p => p.CreatedAt)
            .AsQueryable();

        if (authorId.HasValue)
        {
            query = query.Where(p => p.UserId == authorId.Value);
        }

        var posts = await query
            .Take(30)
            .ToListAsync(cancellationToken);

        var followedIds = currentUserId.HasValue
            ? await db.UserFollows
                .AsNoTracking()
                .Where(f => f.FollowerId == currentUserId.Value)
                .Select(f => f.FollowingId)
                .ToListAsync(cancellationToken)
            : [];

        var suggestedUsers = await db.Users
            .AsNoTracking()
            .Where(u => u.IsActive)
            .OrderByDescending(u => u.Followers)
            .ThenByDescending(u => u.Level)
            .Take(6)
            .Select(u => new SocialSuggestedUserDto
            {
                Id = u.Id,
                Name = u.Name,
                Role = u.Role,
                ProfileImage = u.ProfileImage,
                Followers = u.Followers
            })
            .ToListAsync(cancellationToken);

        foreach (var user in suggestedUsers)
        {
            user.ProfileImage = profileAvatarService.ResolveProfileImage(user.ProfileImage, user.Role);
            user.IsFollowedByCurrentUser = followedIds.Contains(user.Id);
        }

        return new SocialFeedResultDto
        {
            Posts = posts.Select(p => new SocialFeedPostDto
            {
                Id = p.Id,
                AuthorId = p.UserId,
                AuthorName = p.User.Name,
                AuthorRole = p.User.Role,
                AuthorImage = profileAvatarService.ResolveProfileImage(p.User.ProfileImage, p.User.Role),
                Content = p.Content,
                ImageUrl = p.ImageUrl,
                VideoUrl = p.VideoUrl,
                CreatedAt = p.CreatedAt,
                LikeCount = p.Likes.Count,
                CommentCount = p.Comments.Count,
                IsLikedByCurrentUser = currentUserId.HasValue && p.Likes.Any(l => l.UserId == currentUserId.Value),
                Comments = p.Comments
                    .OrderBy(c => c.CreatedAt)
                    .Select(c => new SocialFeedCommentDto
                    {
                        Id = c.Id,
                        UserId = c.UserId,
                        UserName = c.User.Name,
                        UserRole = c.User.Role,
                        UserImage = profileAvatarService.ResolveProfileImage(c.User.ProfileImage, c.User.Role),
                        Content = c.Content,
                        CreatedAt = c.CreatedAt
                    })
                    .ToList()
            }).ToList(),
            SuggestedUsers = suggestedUsers
        };
    }

    public async Task<SocialFeedPostDto> CreatePostAsync(int userId, string content, string? imageUrl = null, string? videoUrl = null, CancellationToken cancellationToken = default)
    {
        var normalizedContent = content.Trim();
        if (string.IsNullOrWhiteSpace(normalizedContent))
        {
            throw new InvalidOperationException("متن پست نمی‌تواند خالی باشد.");
        }

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        var post = new SocialPost
        {
            UserId = userId,
            Content = normalizedContent,
            ImageUrl = string.IsNullOrWhiteSpace(imageUrl) ? null : imageUrl.Trim(),
            VideoUrl = string.IsNullOrWhiteSpace(videoUrl) ? null : videoUrl.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        db.SocialPosts.Add(post);
        await db.SaveChangesAsync(cancellationToken);

        var savedPost = await db.SocialPosts
            .AsNoTracking()
            .Include(p => p.User)
            .FirstAsync(p => p.Id == post.Id, cancellationToken);

        return new SocialFeedPostDto
        {
            Id = savedPost.Id,
            AuthorId = savedPost.UserId,
            AuthorName = savedPost.User.Name,
            AuthorRole = savedPost.User.Role,
            AuthorImage = profileAvatarService.ResolveProfileImage(savedPost.User.ProfileImage, savedPost.User.Role),
            Content = savedPost.Content,
            ImageUrl = savedPost.ImageUrl,
            VideoUrl = savedPost.VideoUrl,
            CreatedAt = savedPost.CreatedAt
        };
    }

    public async Task<SocialFeedCommentDto> AddCommentAsync(int userId, int postId, string content, CancellationToken cancellationToken = default)
    {
        var normalizedContent = content.Trim();
        if (string.IsNullOrWhiteSpace(normalizedContent))
        {
            throw new InvalidOperationException("متن کامنت نمی‌تواند خالی باشد.");
        }

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        var postExists = await db.SocialPosts.AnyAsync(p => p.Id == postId, cancellationToken);
        if (!postExists)
        {
            throw new InvalidOperationException("پست مورد نظر پیدا نشد.");
        }

        var comment = new SocialComment
        {
            PostId = postId,
            UserId = userId,
            Content = normalizedContent,
            CreatedAt = DateTime.UtcNow
        };

        db.SocialComments.Add(comment);
        await db.SaveChangesAsync(cancellationToken);

        var savedComment = await db.SocialComments
            .AsNoTracking()
            .Include(c => c.User)
            .FirstAsync(c => c.Id == comment.Id, cancellationToken);

        return new SocialFeedCommentDto
        {
            Id = savedComment.Id,
            UserId = savedComment.UserId,
            UserName = savedComment.User.Name,
            UserRole = savedComment.User.Role,
            UserImage = profileAvatarService.ResolveProfileImage(savedComment.User.ProfileImage, savedComment.User.Role),
            Content = savedComment.Content,
            CreatedAt = savedComment.CreatedAt
        };
    }

    public async Task<bool> ToggleLikeAsync(int userId, int postId, CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        var existingLike = await db.SocialPostLikes
            .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId, cancellationToken);

        if (existingLike is null)
        {
            db.SocialPostLikes.Add(new SocialPostLike
            {
                PostId = postId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            });

            await db.SaveChangesAsync(cancellationToken);
            return true;
        }

        db.SocialPostLikes.Remove(existingLike);
        await db.SaveChangesAsync(cancellationToken);
        return false;
    }

    public async Task<SocialProfileDto?> GetProfileAsync(int profileUserId, int? currentUserId = null, CancellationToken cancellationToken = default)
    {
        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        var user = await db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == profileUserId && u.IsActive, cancellationToken);

        if (user is null)
        {
            return null;
        }

        var posts = await GetFeedAsync(currentUserId, profileUserId, cancellationToken);
        var isFollowed = currentUserId.HasValue && await db.UserFollows
            .AsNoTracking()
            .AnyAsync(f => f.FollowerId == currentUserId.Value && f.FollowingId == profileUserId, cancellationToken);

        return new SocialProfileDto
        {
            Id = user.Id,
            Name = user.Name,
            Role = user.Role,
            ProfileImage = profileAvatarService.ResolveProfileImage(user.ProfileImage, user.Role),
            Bio = user.Bio,
            Followers = user.Followers,
            Following = user.Following,
            PostCount = posts.Posts.Count,
            IsFollowedByCurrentUser = isFollowed,
            Posts = posts.Posts
        };
    }

    public async Task<bool> ToggleFollowAsync(int followerId, int followingId, CancellationToken cancellationToken = default)
    {
        if (followerId == followingId)
        {
            throw new InvalidOperationException("کاربر نمی‌تواند خودش را دنبال کند.");
        }

        await using var db = await dbFactory.CreateDbContextAsync(cancellationToken);

        var follower = await db.Users.FirstOrDefaultAsync(u => u.Id == followerId, cancellationToken);
        var following = await db.Users.FirstOrDefaultAsync(u => u.Id == followingId, cancellationToken);
        if (follower is null || following is null)
        {
            throw new InvalidOperationException("کاربر مورد نظر پیدا نشد.");
        }

        var relation = await db.UserFollows
            .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowingId == followingId, cancellationToken);

        if (relation is null)
        {
            db.UserFollows.Add(new UserFollow
            {
                FollowerId = followerId,
                FollowingId = followingId,
                CreatedAt = DateTime.UtcNow
            });

            follower.Following++;
            following.Followers++;
            await db.SaveChangesAsync(cancellationToken);
            return true;
        }

        db.UserFollows.Remove(relation);
        follower.Following = Math.Max(0, follower.Following - 1);
        following.Followers = Math.Max(0, following.Followers - 1);
        await db.SaveChangesAsync(cancellationToken);
        return false;
    }
}

public interface ISocialImageStorageService
{
    Task<SocialUploadedMediaDto> SavePostMediaAsync(IBrowserFile file, CancellationToken cancellationToken = default);
    Task<string> SavePostImageAsync(IBrowserFile file, CancellationToken cancellationToken = default);
}

public sealed class SocialUploadedMediaDto
{
    public string Url { get; set; } = string.Empty;
    public string MediaType { get; set; } = "image";
}

public sealed class SocialImageStorageService(IWebHostEnvironment env) : ISocialImageStorageService
{
    private static readonly string[] AllowedImageExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];
    private static readonly string[] AllowedVideoExtensions = [".mp4", ".webm", ".mov"];
    private const long MaxImageBytes = 5L * 1024 * 1024;
    private const long MaxVideoBytes = 100L * 1024 * 1024;

    public async Task<SocialUploadedMediaDto> SavePostMediaAsync(IBrowserFile file, CancellationToken cancellationToken = default)
    {
        var extension = Path.GetExtension(file.Name).ToLowerInvariant();
        var isImage = AllowedImageExtensions.Contains(extension);
        var isVideo = AllowedVideoExtensions.Contains(extension);

        if (!isImage && !isVideo)
        {
            throw new InvalidOperationException("فرمت فایل مجاز نیست.");
        }

        if (isImage && file.Size > MaxImageBytes)
        {
            throw new InvalidOperationException("حجم تصویر نباید بیشتر از 5 مگابایت باشد.");
        }

        if (isVideo && file.Size > MaxVideoBytes)
        {
            throw new InvalidOperationException("حجم ویدیو نباید بیشتر از 100 مگابایت باشد.");
        }

        var uploadsDir = Path.Combine(env.WebRootPath, "uploads", "social");
        Directory.CreateDirectory(uploadsDir);

        var fileName = $"{Guid.NewGuid()}{extension}";
        var fullPath = Path.Combine(uploadsDir, fileName);

        var maxSize = isImage ? MaxImageBytes : MaxVideoBytes;
        await using var source = file.OpenReadStream(maxSize, cancellationToken);
        await using var destination = new FileStream(fullPath, FileMode.Create);
        await source.CopyToAsync(destination, cancellationToken);

        return new SocialUploadedMediaDto
        {
            Url = $"/uploads/social/{fileName}",
            MediaType = isVideo ? "video" : "image"
        };
    }

    public async Task<string> SavePostImageAsync(IBrowserFile file, CancellationToken cancellationToken = default)
    {
        var media = await SavePostMediaAsync(file, cancellationToken);
        if (!string.Equals(media.MediaType, "image", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("فقط تصویر قابل قبول است.");
        }

        return media.Url;
    }
}
