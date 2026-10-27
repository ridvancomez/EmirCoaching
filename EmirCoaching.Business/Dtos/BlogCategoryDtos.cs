using EmirCoaching.Business.Common;

namespace EmirCoaching.Business.Dtos;

// Blog sayfasındaki kategori filtre sekmeleri
public record BlogCategoryResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
}

public record CreateBlogCategoryRequest
{
    public string Name { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
}

public record UpdateBlogCategoryRequest : CreateBlogCategoryRequest, IUpdateRequest
{
    public int Id { get; init; }
}
