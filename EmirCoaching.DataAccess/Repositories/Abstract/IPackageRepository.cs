using EmirCoaching.Entities.Entities;

namespace EmirCoaching.DataAccess.Repositories.Abstract;

public interface IPackageRepository : IRepository<Package>
{
    Task<IReadOnlyList<Package>> GetActiveListAsync();
    Task<Package?> GetBySlugWithDetailsAsync(string slug);

    // Admin güncellemesi için: modüller, kalemler ve öne çıkanlar dahil, takip edilen kayıt
    Task<Package?> GetByIdWithDetailsAsync(int id);
    Task<bool> SlugExistsAsync(string slug, int? excludeId = null);
}
