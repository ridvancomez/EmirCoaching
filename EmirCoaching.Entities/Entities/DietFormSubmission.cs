using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Diyet formu başvurusu (diyet-formu.html). Satın alma akışının ilk adımıdır: kişi paketi seçer, bilgilerini
/// gönderir, sonra Shopier'de öder. Beslenme ve yaşam tarzı ayrıntıları burada değil, koçun ödemeden sonra
/// gönderdiği Google Form'da toplanır. Sağlık verisi içerdiği için açık rıza kaydı (ConsentRecord) tutulur.
/// </summary>
public class DietFormSubmission : BaseEntity
{
    // Seçilen paket. Paket silinirse (soft delete) kayıt kalır; panel paketi soluk gösterir.
    public int PackageId { get; set; }
    public Package Package { get; set; } = null!;

    // 1. Kişisel bilgiler
    public string FullName { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public int Age { get; set; }
    public decimal HeightCm { get; set; }
    public decimal CurrentWeight { get; set; }
    public decimal TargetWeight { get; set; }

    // 2. İletişim bilgileri
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    // Danışanın doldurduğu Google Form yanıtının linki; koç panelden elle girer (sitede yok)
    public string? ClientFormUrl { get; set; }

    public DietFormStatus Status { get; set; }

    public ICollection<ConsentRecord> ConsentRecords { get; set; } = new List<ConsentRecord>();
}
