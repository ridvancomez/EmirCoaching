using EmirCoaching.Business.Common;

namespace EmirCoaching.Business.Dtos;

public record BlogPostResponse
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string Summary { get; init; } = string.Empty;
    public string? CoverImageUrl { get; init; }
    public int ReadingMinutes { get; init; }
    public DateTime? PublishedAt { get; init; }
    public bool IsFeatured { get; init; }
    public string CategoryName { get; init; } = string.Empty;
    public string CategorySlug { get; init; } = string.Empty; // data-category

    // Admin listesi ve düzenleme formu için
    public int CategoryId { get; init; }
    public bool IsPublished { get; init; }
}

public record BlogAuthorResponse
{
    public string FullName { get; init; } = string.Empty;
    public string? Title { get; init; } // yalnızca yazar koçsa dolu; boşsa sitede unvan satırı gösterilmez
    public string? Bio { get; init; }
    public string? AvatarUrl { get; init; }
}

public record BlogPostDetailResponse : BlogPostResponse
{
    public string ContentHtml { get; init; } = string.Empty;
    public string? CoverImageCaption { get; init; }
    public string? MetaTitle { get; init; }
    public string? MetaDescription { get; init; }
    public IReadOnlyList<string> Tags { get; init; } = []; // etiket adları ("#beslenme" olarak gösterilir)
    public BlogAuthorResponse? Author { get; init; }
    public PackageResponse? RecommendedPackage { get; init; } // .oneri-card
    public IReadOnlyList<BlogPostResponse> RelatedPosts { get; init; } = [];

    // Admin düzenleme formu için
    public int? AuthorId { get; init; }
    public int? RecommendedPackageId { get; init; }
}

public record CreateBlogPostRequest
{
    public string Title { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string Summary { get; init; } = string.Empty;
    public string ContentHtml { get; init; } = string.Empty;
    public string? CoverImagePath { get; init; }
    public string? CoverImageCaption { get; init; }
    public int ReadingMinutes { get; init; }
    public bool IsFeatured { get; init; } // işaretlenirse aynı kategorideki önceki öne çıkan kaldırılır
    public List<string> Tags { get; init; } = []; // yoksa oluşturulur
    public int CategoryId { get; init; }
    public int? AuthorId { get; init; }
    public int? RecommendedPackageId { get; init; }
    public string? MetaTitle { get; init; }
    public string? MetaDescription { get; init; }
}

public record UpdateBlogPostRequest : CreateBlogPostRequest, IUpdateRequest
{
    public int Id { get; init; }
}
