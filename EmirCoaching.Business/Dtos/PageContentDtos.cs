namespace EmirCoaching.Business.Dtos;

public record PageContentResponse
{
    public int Id { get; init; }
    public string PageKey { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string? HighlightText { get; init; }
    public string? Description { get; init; }
    public string? BackgroundImageUrl { get; init; }
    public string? ButtonUrl { get; init; }
}

// Sayfalar sabittir (seed ile gelir): yalnızca içerik güncellenir, sayfa PageKey ile seçilir
public record UpdatePageContentRequest
{
    public string Title { get; init; } = string.Empty;
    public string? HighlightText { get; init; }
    public string? Description { get; init; }
    public string? BackgroundImagePath { get; init; }
    public string? ButtonUrl { get; init; }
}
