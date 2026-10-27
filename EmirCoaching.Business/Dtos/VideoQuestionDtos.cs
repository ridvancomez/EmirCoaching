using EmirCoaching.Business.Common;

namespace EmirCoaching.Business.Dtos;

public record VideoQuestionResponse
{
    public int Id { get; init; }
    public string Question { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string ThumbnailUrl { get; init; } = string.Empty;
    public string YouTubeUrl { get; init; } = string.Empty;
    public int SortOrder { get; init; }
    public bool IsActive { get; init; }
}

public record CreateVideoQuestionRequest
{
    public string Question { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string ThumbnailPath { get; init; } = string.Empty;
    public string YouTubeUrl { get; init; } = string.Empty;
    public int SortOrder { get; init; }
    public bool IsActive { get; init; }
}

public record UpdateVideoQuestionRequest : CreateVideoQuestionRequest, IUpdateRequest
{
    public int Id { get; init; }
}
