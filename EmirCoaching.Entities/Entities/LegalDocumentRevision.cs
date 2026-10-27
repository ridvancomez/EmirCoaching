using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Gizlilik sayfasındaki "Revizyon Geçmişi" satırı ([15.10.2024] v2 - Not). Elle girilmez:
/// LegalDocument'in her yeni sürümünde servis tarafından otomatik oluşturulur.
/// Type ayrıca tutulur ki bir türün tüm geçmişi tek sorguyla listelenebilsin.
/// </summary>
public class LegalDocumentRevision : BaseEntity
{
    public LegalDocumentType Type { get; set; }
    public int Version { get; set; }
    public string Note { get; set; } = string.Empty;
    public DateTime RevisedAt { get; set; }

    public int LegalDocumentId { get; set; }
    public LegalDocument LegalDocument { get; set; } = null!;
}
