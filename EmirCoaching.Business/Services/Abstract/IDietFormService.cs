using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Entities.Common;

namespace EmirCoaching.Business.Services.Abstract;

public interface IDietFormService
{
    Task<Result> SubmitAsync(CreateDietFormDto dto, RequestInfo requestInfo);

    // Admin
    Task<PagedResult<DietFormDto>> GetPagedAsync(DietFormStatus? status, int page, int size);
    Task<Result> UpdateStatusAsync(int id, DietFormStatus newStatus);
}
