using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;

namespace EmirCoaching.Business.Services.Abstract;

public interface ITransformationService
{
    // typeSlug: TransformationType.Slug (data-category); sort: "en-yeniler" (varsayılan) | "en-eskiler"
    Task<Result<IReadOnlyList<TransformationResponse>>> GetListAsync(string? typeSlug, string? sort);
    Task<Result<IReadOnlyList<TransformationResponse>>> GetHomeAsync();
    Task<Result<TransformationDetailResponse>> GetDetailAsync(int id);

    // Admin
    Task<Result<IReadOnlyList<TransformationResponse>>> GetAllAsync(); // pasifler dahil
    Task<Result<TransformationDetailResponse>> GetByIdAsync(int id);  // pasif olsa da döner
    Task<Result<int>> CreateAsync(CreateTransformationRequest request);
    Task<Result> UpdateAsync(UpdateTransformationRequest request);
    Task<Result> DeleteAsync(int id);
    Task<Result> DeleteManyAsync(IReadOnlyCollection<int> ids); // ya hepsi ya hiçbiri
}
