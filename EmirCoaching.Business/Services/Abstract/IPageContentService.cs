using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;

namespace EmirCoaching.Business.Services.Abstract;

// Sayfalar seed ile gelir ve sabittir: oluşturma/silme yok, yalnızca içerik güncellenir
public interface IPageContentService
{
    Task<Result<IReadOnlyList<PageContentResponse>>> GetAllAsync();
    Task<Result<PageContentResponse>> GetByPageKeyAsync(string pageKey);
    Task<Result> UpdateAsync(string pageKey, UpdatePageContentRequest request);
}
