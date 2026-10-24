using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;

namespace EmirCoaching.Business.Services.Abstract;

public interface ISiteStatService
{
    Task<IEnumerable<SiteStatDto>> GetAllAsync();
    Task<SiteStatDto?> GetByIdAsync(int id);
    Task<Result<int>> CreateAsync(CreateSiteStatDto dto);
    Task<Result> UpdateAsync(UpdateSiteStatDto dto);
    Task<Result> DeleteAsync(int id);
}
