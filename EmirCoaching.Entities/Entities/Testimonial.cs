using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Paket detay sayfasındaki "Öğrenci Yorumları" kartı. Her yorum bir pakete aittir; paketle birlikte silinir.
/// </summary>
public class Testimonial : BaseEntity, ISortable, IActivatable
{
    public string Name { get; set; } = string.Empty; // "Ahmet Y."
    public string Text { get; set; } = string.Empty;

    // 0–5 arası, 0.5'lik adımlarla (4.5 = dört buçuk yıldız)
    public decimal Rating { get; set; }

    public int SortOrder { get; set; }
    public bool IsActive { get; set; }

    public int PackageId { get; set; }
    public Package Package { get; set; } = null!;
}
