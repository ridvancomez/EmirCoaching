using EmirCoaching.Business.Common;

namespace EmirCoaching.Business.Dtos;

public record SiteStatResponse
{
    public int Id { get; init; }
    public string Label { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
    public string? Suffix { get; init; }
    public int SortOrder { get; init; }
}

public record CreateSiteStatRequest
{
    public string Label { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
    public string? Suffix { get; init; }
    public int SortOrder { get; init; }
}

public record UpdateSiteStatRequest : CreateSiteStatRequest, IUpdateRequest
{
    public int Id { get; init; }
}
