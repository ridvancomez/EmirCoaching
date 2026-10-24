using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;

namespace EmirCoaching.Business.Services.Abstract;

public interface IBlogService
{
    Task<PagedResult<BlogPostDto>> GetPagedAsync(string? categorySlug, int page, int size);
    Task<BlogPostDetailDto?> GetDetailAsync(string slug);

    // Admin
    Task<Result<int>> CreateAsync(CreateBlogPostDto dto);
    Task<Result> UpdateAsync(UpdateBlogPostDto dto);
    Task<Result> DeleteAsync(int id);
    Task<Result> PublishAsync(int id);
}
