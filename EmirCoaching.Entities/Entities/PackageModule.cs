using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Paket detay sayfasındaki modül kartı (Antrenman, Beslenme, Takip &amp; Ölçüm, Koçluk).
/// Kartın "4 Modül" rozeti kilitli olmayan kalem sayısından hesaplanır.
/// </summary>
public class PackageModule : BaseEntity, ISortable
{
    public string Title { get; set; } = string.Empty;
    public string IconKey { get; set; } = string.Empty;
    public int SortOrder { get; set; }

    public int PackageId { get; set; }
    public Package Package { get; set; } = null!;

    public ICollection<PackageModuleItem> Items { get; set; } = new List<PackageModuleItem>();
}
