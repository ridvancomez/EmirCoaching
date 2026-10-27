using EmirCoaching.Business.Common;
using EmirCoaching.Entities.Common;

namespace EmirCoaching.Business.Dtos;

public record FaqResponse
{
    public int Id { get; init; }
    public string Question { get; init; } = string.Empty;
    public string Answer { get; init; } = string.Empty;
    public FaqGroup Group { get; init; }
    public int SortOrder { get; init; }
}

public record CreateFaqRequest
{
    public string Question { get; init; } = string.Empty;
    public string Answer { get; init; } = string.Empty;
    public FaqGroup Group { get; init; }
    public int SortOrder { get; init; }
}

public record UpdateFaqRequest : CreateFaqRequest, IUpdateRequest
{
    public int Id { get; init; }
}
