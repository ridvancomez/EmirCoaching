using EmirCoaching.Entities.Common;

namespace EmirCoaching.Business.Dtos;

public record FaqDto
{
    public int Id { get; init; }
    public string Question { get; init; } = string.Empty;
    public string Answer { get; init; } = string.Empty;
    public FaqGroup Group { get; init; }
    public int SortOrder { get; init; }
}

public record CreateFaqDto
{
    public string Question { get; init; } = string.Empty;
    public string Answer { get; init; } = string.Empty;
    public FaqGroup Group { get; init; }
    public int SortOrder { get; init; }
}

public record UpdateFaqDto : CreateFaqDto
{
    public int Id { get; init; }
}
