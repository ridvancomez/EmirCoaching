using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Koçluk paketi (Başlangıç &amp; Deneme, Dönüşüm Protokolü, VIP Yaşam Tarzı). Her paket Salonda ve Evde
/// olarak ayrı kayıttır, fiyatları farklıdır. Satış Shopier'de yapılır: kişi önce diyet formunu doldurur,
/// "Bilgilerin Alındı" panelindeki düğmeyle PurchaseUrl'e gider.
/// Kullanıldığı yerler: ana sayfa fiyat kartları, paketler sayfası, paket detay, blog detaydaki önerilen paket.
/// </summary>
public class Package : BaseEntity, ISluggable, ISortable, IActivatable
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty; // "6 Aylık Dönüşüm Programı"
    public int DurationMonths { get; set; }
    public decimal Price { get; set; }
    public PackageLocation Location { get; set; }
    public string ShortDescription { get; set; } = string.Empty; // paket detay hero metni ("...6 aylık kapsamlı gelişim planı.")

    // Shopier ürün linki (diyet formu gönderildikten sonraki ödeme düğmesi)
    public string PurchaseUrl { get; set; } = string.Empty;

    // "En Çok Tercih Edilen" rozeti; her Location için en fazla bir paket (filtreli unique index)
    public bool IsFeatured { get; set; }

    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }

    public ICollection<PackageModule> Modules { get; set; } = new List<PackageModule>();
    public ICollection<PackageHighlight> Highlights { get; set; } = new List<PackageHighlight>();
    public ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();
}
