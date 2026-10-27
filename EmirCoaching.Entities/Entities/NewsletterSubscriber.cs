using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Footer'daki "Bültene Katıl" aboneliği. Abonelik ve iptal Brevo'da yönetilir; bu tablo
/// onayın ispatı (KVKK / İYS) için tutulur.
/// </summary>
public class NewsletterSubscriber : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string IpAddress { get; set; } = string.Empty; // ilk abonelik anındaki IP (IP başına günlük sınır için)

    public ICollection<ConsentRecord> ConsentRecords { get; set; } = new List<ConsentRecord>();
}
