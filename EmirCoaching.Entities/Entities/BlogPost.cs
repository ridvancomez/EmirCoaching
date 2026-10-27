using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Identity;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Blog yazısı. İçerik CKEditor'den HTML olarak gelir. Blog sayfasında kart, blog detayda tam makale olarak gösterilir.
/// </summary>
public class BlogPost : BaseEntity, ISluggable
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string ContentHtml { get; set; } = string.Empty;
    public string? CoverImagePath { get; set; }
    public string? CoverImageCaption { get; set; } // blog detayda kapak görselinin altındaki yazı
    public int ReadingMinutes { get; set; }
    public DateTime? PublishedAt { get; set; }
    public bool IsPublished { get; set; }

    // "Öne Çıkan" yazı; her kategoride en fazla bir tane (filtreli unique index).
    // Kategoride işaretli yazı yoksa o kategorinin en son yayınlanan yazısı gösterilir.
    public bool IsFeatured { get; set; }

    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }

    public int CategoryId { get; set; }
    public BlogCategory Category { get; set; } = null!;

    public int? AuthorId { get; set; }
    public AppUser? Author { get; set; }

    // Blog detaydaki "Önerilen" paket kartı; her yazı kendisi seçer
    public int? RecommendedPackageId { get; set; }
    public Package? RecommendedPackage { get; set; }

    public ICollection<BlogPostTag> Tags { get; set; } = new List<BlogPostTag>();
}
