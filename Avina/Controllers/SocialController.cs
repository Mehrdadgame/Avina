using Avina.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Avina.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SocialController(ISocialFeedService socialFeedService) : ControllerBase
{
    [HttpGet("feed")]
    public async Task<ActionResult<SocialFeedResultDto>> GetFeed([FromQuery] int? authorId, CancellationToken cancellationToken)
    {
        var currentUserId = TryGetUserId();
        var result = await socialFeedService.GetFeedAsync(currentUserId, authorId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("profiles/{userId:int}")]
    public async Task<ActionResult<SocialProfileDto>> GetProfile(int userId, CancellationToken cancellationToken)
    {
        var currentUserId = TryGetUserId();
        var profile = await socialFeedService.GetProfileAsync(userId, currentUserId, cancellationToken);
        return profile is null ? NotFound() : Ok(profile);
    }

    [Authorize]
    [HttpPost("posts")]
    public async Task<ActionResult<SocialFeedPostDto>> CreatePost([FromBody] CreateSocialPostRequest request, CancellationToken cancellationToken)
    {
        var userId = RequireUserId();
        var post = await socialFeedService.CreatePostAsync(userId, request.Content, request.ImageUrl, request.VideoUrl, cancellationToken);
        return Ok(post);
    }

    [Authorize]
    [HttpPost("posts/{postId:int}/likes/toggle")]
    public async Task<ActionResult<ToggleLikeResponse>> ToggleLike(int postId, CancellationToken cancellationToken)
    {
        var userId = RequireUserId();
        var isLiked = await socialFeedService.ToggleLikeAsync(userId, postId, cancellationToken);
        return Ok(new ToggleLikeResponse(isLiked));
    }

    [Authorize]
    [HttpPost("posts/{postId:int}/reactions")]
    public async Task<ActionResult<SetReactionResponse>> SetReaction(int postId, [FromBody] SetReactionRequest request, CancellationToken cancellationToken)
    {
        var userId = RequireUserId();
        var result = await socialFeedService.SetReactionAsync(userId, postId, request.Reaction, cancellationToken);
        return Ok(new SetReactionResponse(result));
    }

    [Authorize]
    [HttpPost("users/{userId:int}/follow-toggle")]
    public async Task<ActionResult<ToggleFollowResponse>> ToggleFollow(int userId, CancellationToken cancellationToken)
    {
        var followerId = RequireUserId();
        var isFollowing = await socialFeedService.ToggleFollowAsync(followerId, userId, cancellationToken);
        return Ok(new ToggleFollowResponse(isFollowing));
    }

    private int? TryGetUserId()
    {
        var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        return int.TryParse(claim?.Value, out var userId) ? userId : null;
    }

    private int RequireUserId()
    {
        var userId = TryGetUserId();
        if (!userId.HasValue)
        {
            throw new UnauthorizedAccessException();
        }

        return userId.Value;
    }
}

public record CreateSocialPostRequest(string Content, string? ImageUrl, string? VideoUrl);
public record ToggleLikeResponse(bool IsLiked);
public record ToggleFollowResponse(bool IsFollowing);
public record SetReactionRequest(Avina.Models.ReactionType? Reaction);
public record SetReactionResponse(Avina.Models.ReactionType? Reaction);
