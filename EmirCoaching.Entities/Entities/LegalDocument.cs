using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

public class LegalDocument : BaseEntity
{
    public LegalDocumentType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Version { get; set; }
    public string? FilePath { get; set; }
    public string? ContentHtml { get; set; }
    public DateTime PublishedAt { get; set; }
    public bool IsCurrent { get; set; }

    public ICollection<ConsentRecord> ConsentRecords { get; set; } = new List<ConsentRecord>();
}
