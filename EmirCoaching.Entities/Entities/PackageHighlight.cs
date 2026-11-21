using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Paket kartındaki özellik satırı (paketler sayfası ve ana sayfa "Bu Pakette Neler Var").
/// Paketler arasındaki tek içerik farkı kilitli satırdır (örn. Zoom görüşmesi 3 aylıkta kilitli).
/// "Pakete Özel" / "6 ve 12 Aylık Pakette" etiket metinleri front-end'de sabittir.
/// </summary>
public class PackageHighlight : BaseEntity, ISortable
{
    public string Text { get; set; } = string.Empty;
    public string? SubText { get; set; } // alt satır, ör. "Her Cumartesi · 20 dk birebir"
    public string IconKey { get; set; } = string.Empty; // front-end ikon eşlemesi: antrenman, beslenme, tahlil, takvim, whatsapp, zoom
    public bool IsLocked { get; set; } // paketler-card__feature--kilitli
    public int SortOrder { get; set; }

    public int PackageId { get; set; }
    public Package Package { get; set; } = null!;
}
