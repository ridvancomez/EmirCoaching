using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Entities.Common;

namespace EmirCoaching.Business.Services.Abstract;

public interface IDietFormSubmissionService
{
    Task<Result> SubmitAsync(CreateDietFormSubmissionRequest request, RequestInfo requestInfo);

    // Admin
    Task<Result<PagedResult<DietFormSubmissionResponse>>> GetPagedAsync(DietFormStatus? status, int page, int size);
    Task<Result> UpdateStatusAsync(int id, DietFormStatus newStatus);
    Task<Result> DeleteAsync(int id); // kalıcı silme (KVKK)
}
