using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// İstatistik maddesi ("8+ Yıl Deneyim", "100+ Danışan", "%95 Başarı").
/// Yalnızca Hakkımda hero'sunda gösterilir (ana sayfa ve değişimler şeridi kaldırıldı).
/// </summary>
public class SiteStat : BaseEntity, ISortable
{
    public string Label { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Suffix { get; set; }
    public int SortOrder { get; set; }
}
