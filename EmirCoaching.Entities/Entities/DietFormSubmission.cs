using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Diyet formu başvurusu (diyet-formu.html). Gönderilince veritabanına yazılır ve ayarlardaki
/// adrese e-posta ile bildirilir. Sağlık verisi içerdiği için açık rıza kaydı (ConsentRecord) zorunludur.
/// </summary>
public class DietFormSubmission : BaseEntity
{
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

    // 3. Beslenme alışkanlıkları
    public int MealsPerDay { get; set; }
    public string? Allergies { get; set; }
    public string? AvoidedFoods { get; set; }
    public string? Supplements { get; set; }

    // 4. Yaşam tarzı
    public int? SleepHours { get; set; }
    public ActivityLevel ActivityLevel { get; set; }
    public decimal? WaterLiters { get; set; }

    public DietFormStatus Status { get; set; }

    public ICollection<ConsentRecord> ConsentRecords { get; set; } = new List<ConsentRecord>();
}
