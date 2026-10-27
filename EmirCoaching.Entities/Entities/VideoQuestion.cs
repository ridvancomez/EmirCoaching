using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Ana sayfadaki videolu soru-cevap slider'ının kartı (.qa__card): soru, kısa cevap,
/// kapak görseli ve oynat butonuyla açılan YouTube videosu.
/// </summary>
public class VideoQuestion : BaseEntity, ISortable, IActivatable
{
    public string Question { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ThumbnailPath { get; set; } = string.Empty;
    public string YouTubeUrl { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
}
