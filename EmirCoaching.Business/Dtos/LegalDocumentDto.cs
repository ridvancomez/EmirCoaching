using EmirCoaching.Entities.Common;

namespace EmirCoaching.Business.Dtos;

public record LegalDocumentDto
{
    public int Id { get; init; }
    public LegalDocumentType Type { get; init; }
    public string Title { get; init; } = string.Empty;
    public int Version { get; init; }
    public string? FilePath { get; init; }
    public string? ContentHtml { get; init; }
    public DateTime PublishedAt { get; init; }
}
