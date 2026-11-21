using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Ana sayfadaki "Satın aldıktan sonra NE OLUYOR?" adımı (01 Form + Kan Tahlili ... 04 Revize).
/// Diyet planı sayfasındaki "Nasıl Çalışır?" adımından (DietProgramStep) ayrıdır: süre etiketi vardır.
/// Adım numarası SortOrder sırasından gelir.
/// </summary>
public class PurchaseStep : BaseEntity, ISortable
{
    public string Title { get; set; } = string.Empty;       // "48 Saatte Program"
    public string Description { get; set; } = string.Empty;
    public string TimeLabel { get; set; } = string.Empty;   // "En geç 48 saat"
    public string IconKey { get; set; } = string.Empty;     // front-end ikon eşlemesi: form, saat, takvim, revize
    public int SortOrder { get; set; }
}
