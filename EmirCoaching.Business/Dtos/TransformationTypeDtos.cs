using EmirCoaching.Business.Common;

namespace EmirCoaching.Business.Dtos;

// Değişimler sayfasındaki filtre sekmeleri
public record TransformationTypeResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public int SortOrder { get; init; }
}

public record CreateTransformationTypeRequest
{
    public string Name { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public int SortOrder { get; init; }
}

public record UpdateTransformationTypeRequest : CreateTransformationTypeRequest, IUpdateRequest
{
    public int Id { get; init; }
}
