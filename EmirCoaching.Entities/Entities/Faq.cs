using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Sıkça sorulan soru; ana sayfadaki akordeon SSS bölümünde listelenir.
/// </summary>
public class Faq : BaseEntity, ISortable
{
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public FaqGroup Group { get; set; }
    public int SortOrder { get; set; }
}
