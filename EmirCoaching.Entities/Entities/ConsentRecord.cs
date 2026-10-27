using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Kullanıcının verdiği onayın kaydı (gizlilik, sağlık verisi açık rızası, ticari ileti).
/// Onay anındaki yasal metin sürümüne bağlanır ve tam olarak bir form kaydına aittir
/// (iletişim mesajı, diyet formu veya bülten aboneliği).
/// </summary>
public class ConsentRecord : BaseEntity
{
    public ConsentType ConsentType { get; set; }
    public bool IsGranted { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public DateTime GrantedAt { get; set; }
    public string? IysStatus { get; set; }

    public int? LegalDocumentId { get; set; }
    public LegalDocument? LegalDocument { get; set; }

    public int? ContactMessageId { get; set; }
    public ContactMessage? ContactMessage { get; set; }

    public int? DietFormSubmissionId { get; set; }
    public DietFormSubmission? DietFormSubmission { get; set; }

    public int? NewsletterSubscriberId { get; set; }
    public NewsletterSubscriber? NewsletterSubscriber { get; set; }
}
