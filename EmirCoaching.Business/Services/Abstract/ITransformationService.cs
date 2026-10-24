using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Entities.Common;

namespace EmirCoaching.Business.Services.Abstract;

public interface ITransformationService
{
    // sort: "en-yeniler" (varsayılan) | "en-eskiler"
    Task<IEnumerable<TransformationDto>> GetListAsync(TransformationCategory? category, string? sort);
    Task<IEnumerable<TransformationDto>> GetHomeAsync();
    Task<TransformationDetailDto?> GetDetailAsync(int id);

    // Admin
    Task<Result<int>> CreateAsync(CreateTransformationDto dto);
    Task<Result> UpdateAsync(UpdateTransformationDto dto);
    Task<Result> DeleteAsync(int id);
}
