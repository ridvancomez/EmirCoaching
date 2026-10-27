using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Sosyal medya hesabı linki. İletişim sayfasındaki 4 kart (açıklama metniyle), footer ikonları ve
/// sitedeki tüm "WhatsApp İletişim" butonları buradan beslenir. Her platform için tek kayıt olur.
/// </summary>
public class SocialLink : BaseEntity, ISortable, IActivatable
{
    public SocialPlatform Platform { get; set; }
    public string Url { get; set; } = string.Empty;         // WhatsApp için https://wa.me/90...
    public string Description { get; set; } = string.Empty; // "Hızlı destek ve program danışmanlığı için anında ulaş."
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
}
