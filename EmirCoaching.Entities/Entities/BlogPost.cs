using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Identity;

namespace EmirCoaching.Entities.Entities;

public class BlogPost : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string ContentHtml { get; set; } = string.Empty;
    public string? CoverImagePath { get; set; }
    public int ReadingMinutes { get; set; }
    public DateTime? PublishedAt { get; set; }
    public bool IsPublished { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }

    public int CategoryId { get; set; }
    public BlogCategory Category { get; set; } = null!;

    public int? AuthorId { get; set; }
    public AppUser? Author { get; set; }

    // Blog detaydaki öneri kartı (.oneri-card)
    public int? RecommendedPackageId { get; set; }
    public Package? RecommendedPackage { get; set; }
}
