using Microsoft.AspNetCore.Components.Forms;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace Avina.Services;

public sealed class ProfileAvatarCropOptions
{
    public double Zoom { get; set; } = 1;
    public double OffsetX { get; set; }
    public double OffsetY { get; set; }
}

public interface IProfileAvatarService
{
    string NormalizeRole(string? role);
    string GetDefaultAvatar(string? role);
    string ResolveProfileImage(string? profileImage, string? role);
    bool IsDefaultAvatar(string? profileImage);
    Task<string> SaveProfileImageAsync(IBrowserFile file, ProfileAvatarCropOptions? cropOptions = null, CancellationToken cancellationToken = default);
}

public sealed class ProfileAvatarService(IWebHostEnvironment env) : IProfileAvatarService
{
    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];

    public string NormalizeRole(string? role)
    {
        var normalized = role?.Trim();
        if (string.IsNullOrWhiteSpace(normalized))
        {
            return "دانش‌آموز";
        }

        if (normalized.Contains("معلم", StringComparison.OrdinalIgnoreCase) ||
            normalized.Contains("استاد", StringComparison.OrdinalIgnoreCase) ||
            normalized.Contains("مدرس", StringComparison.OrdinalIgnoreCase) ||
            normalized.Contains("teacher", StringComparison.OrdinalIgnoreCase))
        {
            return "معلم";
        }

        if (normalized.Contains("دانش", StringComparison.OrdinalIgnoreCase) ||
            normalized.Contains("student", StringComparison.OrdinalIgnoreCase))
        {
            return "دانش‌آموز";
        }

        return normalized;
    }

    public string GetDefaultAvatar(string? role)
    {
        var normalizedRole = NormalizeRole(role);
        return normalizedRole == "معلم"
            ? "/images/avatars/teacher-default.svg"
            : "/images/avatars/student-default.svg";
    }

    public string ResolveProfileImage(string? profileImage, string? role)
    {
        return string.IsNullOrWhiteSpace(profileImage)
            ? GetDefaultAvatar(role)
            : profileImage;
    }

    public bool IsDefaultAvatar(string? profileImage)
    {
        return !string.IsNullOrWhiteSpace(profileImage) &&
               profileImage.StartsWith("/images/avatars/", StringComparison.OrdinalIgnoreCase);
    }

    public async Task<string> SaveProfileImageAsync(IBrowserFile file, ProfileAvatarCropOptions? cropOptions = null, CancellationToken cancellationToken = default)
    {
        var extension = Path.GetExtension(file.Name).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
        {
            throw new InvalidOperationException("فرمت تصویر پروفایل مجاز نیست.");
        }

        await using var input = file.OpenReadStream(10 * 1024 * 1024, cancellationToken);
        using var image = await Image.LoadAsync(input, cancellationToken);

        ApplyCrop(image, cropOptions);

        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Size = new Size(512, 512),
            Mode = ResizeMode.Crop,
            Sampler = KnownResamplers.Lanczos3
        }));

        var uploadsDir = Path.Combine(env.WebRootPath, "uploads", "profiles");
        Directory.CreateDirectory(uploadsDir);

        var fileName = $"{Guid.NewGuid()}.webp";
        var fullPath = Path.Combine(uploadsDir, fileName);

        await image.SaveAsync(fullPath, new WebpEncoder
        {
            Quality = 90,
            FileFormat = WebpFileFormatType.Lossy
        }, cancellationToken);

        return $"/uploads/profiles/{fileName}";
    }

    private static void ApplyCrop(Image image, ProfileAvatarCropOptions? cropOptions)
    {
        if (cropOptions is null)
        {
            var baseSize = Math.Min(image.Width, image.Height);
            var centerCropX = Math.Max(0, (image.Width - baseSize) / 2);
            var centerCropY = Math.Max(0, (image.Height - baseSize) / 2);
            image.Mutate(op => op.Crop(new Rectangle(centerCropX, centerCropY, baseSize, baseSize)));
            return;
        }

        var zoom = Math.Clamp(cropOptions.Zoom, 1, 3);
        var shortest = Math.Min(image.Width, image.Height);
        var cropSize = Math.Max(64, (int)Math.Round(shortest / zoom));
        cropSize = Math.Min(cropSize, shortest);

        var centerX = image.Width / 2d + Math.Clamp(cropOptions.OffsetX, -1, 1) * (image.Width - cropSize) / 2d;
        var centerY = image.Height / 2d + Math.Clamp(cropOptions.OffsetY, -1, 1) * (image.Height - cropSize) / 2d;

        var finalCropX = (int)Math.Round(centerX - cropSize / 2d);
        var finalCropY = (int)Math.Round(centerY - cropSize / 2d);

        finalCropX = Math.Clamp(finalCropX, 0, Math.Max(0, image.Width - cropSize));
        finalCropY = Math.Clamp(finalCropY, 0, Math.Max(0, image.Height - cropSize));

        image.Mutate(op => op.Crop(new Rectangle(finalCropX, finalCropY, cropSize, cropSize)));
    }
}
