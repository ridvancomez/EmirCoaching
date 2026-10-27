using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;

namespace EmirCoaching.Business.Services.Abstract;

public interface IPackageService
{
    Task<Result<IReadOnlyList<PackageResponse>>> GetListAsync();
    Task<Result<PackageDetailResponse>> GetDetailAsync(string slug);

    // Admin
    Task<Result<IReadOnlyList<PackageResponse>>> GetAllAsync(); // pasifler dahil
    Task<Result<PackageDetailResponse>> GetByIdAsync(int id);
    Task<Result<int>> CreateAsync(CreatePackageRequest request);
    Task<Result> UpdateAsync(UpdatePackageRequest request);
    Task<Result> DeleteAsync(int id);
    Task<Result> ReorderModulesAsync(int packageId, List<int> moduleIds);
}
