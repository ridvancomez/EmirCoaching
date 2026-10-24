namespace EmirCoaching.Business.Dtos;

public record SiteStatDto
{
    public int Id { get; init; }
    public string Label { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
    public string? Suffix { get; init; }
    public int SortOrder { get; init; }
}

public record CreateSiteStatDto
{
    public string Label { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
    public string? Suffix { get; init; }
    public int SortOrder { get; init; }
}

public record UpdateSiteStatDto : CreateSiteStatDto
{
    public int Id { get; init; }
}
