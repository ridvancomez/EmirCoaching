using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;

namespace EmirCoaching.Business.Services.Abstract;

public interface IPackageService
{
    Task<IEnumerable<PackageDto>> GetListAsync();
    Task<PackageDetailDto?> GetDetailAsync(string slug);

    // Admin
    Task<Result<int>> CreateAsync(CreatePackageDto dto);
    Task<Result> UpdateAsync(UpdatePackageDto dto);
    Task<Result> DeleteAsync(int id);
    Task<Result> ReorderModulesAsync(int packageId, List<int> moduleIds);
}
