using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Ana sayfadaki "Bizi Sosyal Medyadan Tanıyın" bölümündeki post/video kartı.
/// Instagram kayıtları Meta Graph API'den otomatik çekilir (son 3 post, ExternalId ile eşleşir);
/// YouTube kayıtları panelden elle girilir.
/// </summary>
public class SocialPost : BaseEntity, ISortable, IActivatable
{
    public SocialPlatform Platform { get; set; }
    public string? ExternalId { get; set; } // Instagram media id; elle girilen kayıtlarda boş
    public string? Title { get; set; }      // YouTube: "Kardionun Önemi"
    public string? Caption { get; set; }
    public string? Hashtag { get; set; }
    public string ImagePath { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int? LikeCount { get; set; }
    public DateTime? PostedAt { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
}
