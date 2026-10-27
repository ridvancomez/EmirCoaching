using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Değişim kartındaki ölçüm satırı (Ağırlık: -12 KG, Yağ Oranı: -%8, Kas Kütlesi: +2 KG).
/// Değer serbest metindir ("Sabit" gibi değerler de girilebilir).
/// </summary>
public class TransformationMetric : BaseEntity, ISortable
{
    public string Label { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public int SortOrder { get; set; }

    public int TransformationId { get; set; }
    public Transformation Transformation { get; set; } = null!;
}
