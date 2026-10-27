using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Sayfanın üst bölümünün (hero / breadcrumb alanı) içeriği: başlık, vurgulu kelime, açıklama ve arka plan görseli.
/// Her sayfa için tek kayıt (PageKey). Ana sayfa slider'ı da buradadır ("home"): "Hemen Başla" butonunun
/// gideceği adres ButtonUrl'dir, WhatsApp butonu sabittir.
/// </summary>
public class PageContent : BaseEntity
{
    public string PageKey { get; set; } = string.Empty; // "home", "paketler", "blog", "degisimler"...
    public string Title { get; set; } = string.Empty;    // başlığın tamamı, vurgulu kelime dahil
    public string? HighlightText { get; set; }           // Title içinde vurgulanacak kısım (örn. "GÜÇLÜ")
    public string? Description { get; set; }
    public string? BackgroundImagePath { get; set; }
    public string? ButtonUrl { get; set; }
}
