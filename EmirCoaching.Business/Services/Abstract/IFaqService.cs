using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Entities.Common;

namespace EmirCoaching.Business.Services.Abstract;

public interface IFaqService
{
    Task<IEnumerable<FaqDto>> GetByGroupAsync(FaqGroup group);
    Task<IEnumerable<FaqDto>> GetAllAsync();
    Task<FaqDto?> GetByIdAsync(int id);
    Task<Result<int>> CreateAsync(CreateFaqDto dto);
    Task<Result> UpdateAsync(UpdateFaqDto dto);
    Task<Result> DeleteAsync(int id);
}
