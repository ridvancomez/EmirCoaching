using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

// Onay kaydı yasal delildir: üç sahip FK'dan (ContactMessage / DietFormSubmission /
// NewsletterSubscriber) tam biri dolu olur — check constraint ile zorlanır.
public class ConsentRecord : BaseEntity
{
    public ConsentType ConsentType { get; set; }
    public bool IsGranted { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public DateTime GrantedAt { get; set; }
    public string? IysStatus { get; set; }

    // Onaylanan metnin sürümü
    public int? LegalDocumentId { get; set; }
    public LegalDocument? LegalDocument { get; set; }

    public int? ContactMessageId { get; set; }
    public ContactMessage? ContactMessage { get; set; }

    public int? DietFormSubmissionId { get; set; }
    public DietFormSubmission? DietFormSubmission { get; set; }

    public int? NewsletterSubscriberId { get; set; }
    public NewsletterSubscriber? NewsletterSubscriber { get; set; }
}
