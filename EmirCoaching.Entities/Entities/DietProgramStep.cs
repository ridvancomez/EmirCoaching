using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Diyet planı detay sayfasındaki "Nasıl Çalışır?" adımı (1. Formu Doldur, 2. Planın Hazırlanır...).
/// Program tek kayıt olduğu için ona bağlı değildir; sayfanın kendi içeriğidir.
/// </summary>
public class DietProgramStep : BaseEntity, ISortable
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DietProgramStepIcon Icon { get; set; }
    public int SortOrder { get; set; }
}
