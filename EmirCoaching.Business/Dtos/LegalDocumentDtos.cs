using EmirCoaching.Entities.Common;

namespace EmirCoaching.Business.Dtos;

public record LegalDocumentResponse
{
    public int Id { get; init; }
    public LegalDocumentType Type { get; init; }
    public string Title { get; init; } = string.Empty;
    public int Version { get; init; }
    public string Note { get; init; } = string.Empty;
    public string? FileUrl { get; init; }
    public string? ContentHtml { get; init; }
    public DateTime PublishedAt { get; init; }
}

// Gizlilik sayfasındaki "Revizyon Geçmişi" satırı
public record LegalDocumentRevisionResponse
{
    public int Version { get; init; }
    public string Note { get; init; } = string.Empty;
    public DateTime RevisedAt { get; init; }
}
