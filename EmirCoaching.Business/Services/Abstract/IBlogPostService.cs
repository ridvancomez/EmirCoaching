using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;

namespace EmirCoaching.Business.Services.Abstract;

public interface IBlogPostService
{
    Task<Result<PagedResult<BlogPostResponse>>> GetPagedAsync(string? categorySlug, int page, int size);
    Task<Result<BlogPostDetailResponse>> GetDetailAsync(string slug);

    // Blog sayfasındaki "Öne Çıkan" kart (kategori filtresine göre); yayında yazı yoksa NotFound
    Task<Result<BlogPostResponse>> GetFeaturedAsync(string? categorySlug);

    // Admin
    Task<Result<PagedResult<BlogPostResponse>>> GetAllPagedAsync(int? categoryId, bool? isPublished, int page, int size); // taslaklar dahil
    Task<Result<BlogPostDetailResponse>> GetByIdAsync(int id); // taslak olsa da döner
    Task<Result<int>> CreateAsync(CreateBlogPostRequest request);
    Task<Result> UpdateAsync(UpdateBlogPostRequest request);
    Task<Result> DeleteAsync(int id);
    Task<Result> DeleteManyAsync(IReadOnlyCollection<int> ids); // ya hepsi ya hiçbiri
    Task<Result> PublishAsync(int id);
    Task<Result> UnpublishAsync(int id); // taslağa döner; ilk yayın tarihi korunur
}
