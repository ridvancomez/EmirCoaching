using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Her sayfadaki "BİZE YAZIN" modalından gelen mesaj ("Hemen Başla" butonlarıyla açılır).
/// Seçilen paket + antrenman yeri ilgili Package kaydına çözülür.
/// </summary>
public class ContactMessage : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public PackageLocation? Location { get; set; } // "ANTRENMAN YERİ"
    public string? Message { get; set; }
    public ContactMessageStatus Status { get; set; }

    public int? PackageId { get; set; }
    public Package? Package { get; set; }

    public ICollection<ConsentRecord> ConsentRecords { get; set; } = new List<ConsentRecord>();
}
