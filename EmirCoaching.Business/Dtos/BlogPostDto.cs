namespace EmirCoaching.Business.Dtos;

public record BlogPostDto
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string Summary { get; init; } = string.Empty;
    public string? CoverImagePath { get; init; }
    public int ReadingMinutes { get; init; }
    public DateTime? PublishedAt { get; init; }
    public string CategoryName { get; init; } = string.Empty;
    public string CategorySlug { get; init; } = string.Empty; // data-category
}

public record BlogAuthorDto
{
    public string FullName { get; init; } = string.Empty;
    public string? Title { get; init; }
    public string? Bio { get; init; }
    public string? AvatarPath { get; init; }
}

public record BlogPostDetailDto : BlogPostDto
{
    public string ContentHtml { get; init; } = string.Empty;
    public string? MetaTitle { get; init; }
    public string? MetaDescription { get; init; }
    public BlogAuthorDto? Author { get; init; }
    public PackageDto? RecommendedPackage { get; init; } // .oneri-card
    public IReadOnlyList<BlogPostDto> RelatedPosts { get; init; } = [];
}

public record CreateBlogPostDto
{
    public string Title { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string Summary { get; init; } = string.Empty;
    public string ContentHtml { get; init; } = string.Empty;
    public string? CoverImagePath { get; init; }
    public int ReadingMinutes { get; init; }
    public int CategoryId { get; init; }
    public int? AuthorId { get; init; }
    public int? RecommendedPackageId { get; init; }
    public string? MetaTitle { get; init; }
    public string? MetaDescription { get; init; }
}

public record UpdateBlogPostDto : CreateBlogPostDto
{
    public int Id { get; init; }
}
