using EmirCoaching.Business.Common;
using EmirCoaching.Entities.Common;

namespace EmirCoaching.Business.Dtos;

public record SocialLinkResponse
{
    public int Id { get; init; }
    public SocialPlatform Platform { get; init; }
    public string Url { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int SortOrder { get; init; }
    public bool IsActive { get; init; }
}

// Her platformdan tek kayıt olabilir
public record CreateSocialLinkRequest
{
    public SocialPlatform Platform { get; init; }
    public string Url { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int SortOrder { get; init; }
    public bool IsActive { get; init; }
}

public record UpdateSocialLinkRequest : CreateSocialLinkRequest, IUpdateRequest
{
    public int Id { get; init; }
}
