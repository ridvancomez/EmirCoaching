using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Yasal metnin bir sürümü (KVKK, açık rıza, çerez, iptal/iade, ticari ileti, gizlilik, mesafeli satış).
/// İçerik panelde zengin metin editörüyle (CKEditor) girilir ve ContentHtml'de tutulur; FilePath yalnızca
/// ilk (seed) sürümlerin PDF'leri içindir. Yeni sürüm yayınlanınca Version otomatik artar (sitede "v3"),
/// PublishedAt otomatik atanır ve Note ile birlikte LegalDocumentRevision tablosuna da işlenir.
/// Eski onaylar eski sürüme bağlı kalır.
/// </summary>
public class LegalDocument : BaseEntity
{
    public LegalDocumentType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Version { get; set; }
    public string Note { get; set; } = string.Empty; // "Çerez politikası entegrasyonu güncellendi."
    public string? FilePath { get; set; }
    public string? ContentHtml { get; set; }
    public DateTime PublishedAt { get; set; }        // yürürlük tarihi olarak da gösterilir
    public bool IsCurrent { get; set; }

    public ICollection<LegalDocumentRevision> Revisions { get; set; } = new List<LegalDocumentRevision>();
    public ICollection<ConsentRecord> ConsentRecords { get; set; } = new List<ConsentRecord>();
}
