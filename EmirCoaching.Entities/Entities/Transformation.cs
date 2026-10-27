using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Danışan önce/sonra değişimi. Değişimler sayfasında (admin'in seçtiği bölümde: carousel veya galeri),
/// ana sayfada (ShowOnHome işaretliler, yeniden eskiye) ve hikaye modalında gösterilir.
/// Ana sayfadaki fotoğraf bulanıklaştırma front-end'de JS ile yapılır, burada tutulmaz.
/// </summary>
public class Transformation : BaseEntity, ISortable, IActivatable
{
    public string ClientName { get; set; } = string.Empty;
    public int Age { get; set; }
    public int DurationWeeks { get; set; } // dönüşüm süresi ("12 Hafta")
    public string Quote { get; set; } = string.Empty;
    public string Story { get; set; } = string.Empty; // "Hikayeyi Oku" modalındaki uzun metin
    public string BeforeImagePath { get; set; } = string.Empty;
    public string AfterImagePath { get; set; } = string.Empty;
    public string? BeforeSquarePath { get; set; }
    public string? AfterSquarePath { get; set; }
    public int Year { get; set; }         // data-yil
    public bool AlignBottom { get; set; } // compare--alta-yasli
    public TransformationSection Section { get; set; }
    public bool ShowOnHome { get; set; }
    public bool IsActive { get; set; } // pasif değişim sitede hiçbir yerde görünmez
    public int SortOrder { get; set; }

    public int TransformationTypeId { get; set; }
    public TransformationType TransformationType { get; set; } = null!;

    public ICollection<TransformationMetric> Metrics { get; set; } = new List<TransformationMetric>();
}
