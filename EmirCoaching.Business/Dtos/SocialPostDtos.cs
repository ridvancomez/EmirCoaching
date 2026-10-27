using EmirCoaching.Business.Common;
using EmirCoaching.Entities.Common;

namespace EmirCoaching.Business.Dtos;

public record SocialPostResponse
{
    public int Id { get; init; }
    public SocialPlatform Platform { get; init; }
    public string? Title { get; init; }
    public string? Caption { get; init; }
    public string? Hashtag { get; init; }
    public string ImageUrl { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
    public int? LikeCount { get; init; }
    public DateTime? PostedAt { get; init; }
    public int SortOrder { get; init; }
    public bool IsActive { get; init; }
}

// Elle girilen postlar (YouTube). Instagram postları Meta senkronuyla gelir; ExternalId buradan girilmez.
public record CreateSocialPostRequest
{
    public SocialPlatform Platform { get; init; }
    public string? Title { get; init; }
    public string? Caption { get; init; }
    public string? Hashtag { get; init; }
    public string ImagePath { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
    public DateTime? PostedAt { get; init; }
    public int SortOrder { get; init; }
    public bool IsActive { get; init; }
}

public record UpdateSocialPostRequest : CreateSocialPostRequest, IUpdateRequest
{
    public int Id { get; init; }
}
