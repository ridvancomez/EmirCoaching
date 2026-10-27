using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Sitenin genel ayarları; tablo tek satırdır ve her zaman aynı kayıt güncellenir.
/// Sosyal medya linkleri (WhatsApp dahil) burada değil, SocialLink tablosundadır.
/// </summary>
public class SiteSetting : BaseEntity
{
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string LegalEmail { get; set; } = string.Empty; // yasal başvurular (legal@...)
    public string Address { get; set; } = string.Empty;

    // Footer "Çalışma saatleri": iki satır, her satırda gün aralığı + saat aralığı
    public string WorkingDays1 { get; set; } = string.Empty;  // "Pazartesi - Cumartesi"
    public string WorkingHours1 { get; set; } = string.Empty; // "08:00 - 22:00"
    public string WorkingDays2 { get; set; } = string.Empty;  // "Pazar"
    public string WorkingHours2 { get; set; } = string.Empty; // "10:00 - 20:00"
}
