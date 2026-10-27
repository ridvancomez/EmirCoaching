using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Dönüşüm tipi (Yağ Kaybı, Kas Kazanımı, Kondisyon &amp; Yağ Kaybı). Değişim kartlarındaki etiket
/// ve değişimler sayfasındaki kategori filtresi buradan gelir; panelden yeni tip eklenebilir.
/// </summary>
public class TransformationType : BaseEntity, ISluggable, ISortable
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty; // filtre değeri (data-category)
    public int SortOrder { get; set; }

    public ICollection<Transformation> Transformations { get; set; } = new List<Transformation>();
}
