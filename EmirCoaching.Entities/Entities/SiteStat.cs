using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Sayaç istatistiği (100+ Mutlu Danışan, %95 Başarı Oranı, 8+ Yıllık Deneyim...).
/// Ana sayfa, değişimler ve hakkımda sayfalarında gösterilir.
/// </summary>
public class SiteStat : BaseEntity, ISortable
{
    public string Label { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Suffix { get; set; }
    public int SortOrder { get; set; }
}
