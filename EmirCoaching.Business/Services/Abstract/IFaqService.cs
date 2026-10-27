using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Entities.Common;

namespace EmirCoaching.Business.Services.Abstract;

public interface IFaqService : ICrudService<FaqResponse, CreateFaqRequest, UpdateFaqRequest>
{
    Task<Result<IReadOnlyList<FaqResponse>>> GetByGroupAsync(FaqGroup group);
}
