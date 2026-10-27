using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;

namespace EmirCoaching.DataAccess.Repositories.Abstract;

public interface IPackageRepository : ISlugRepository<Package>
{
    Task<IReadOnlyList<Package>> GetActiveListAsync();
    Task<Package?> GetBySlugWithDetailsAsync(string slug);

    // Admin listesi: pasifler dahil, sitedeki sırayla
    Task<IReadOnlyList<Package>> GetOrderedListAsync();

    // Admin düzenleme formu için: modüller, kalemler, öne çıkanlar ve tüm yorumlar sıralı, takip edilmeyen kayıt
    Task<Package?> GetDetailByIdAsync(int id);

    // Admin güncellemesi için: modüller, kalemler ve öne çıkanlar dahil, takip edilen kayıt
    Task<Package?> GetByIdWithDetailsAsync(int id);

    // Kategorideki "En Çok Tercih Edilen" paket (takip edilen kayıt)
    Task<Package?> GetFeaturedAsync(PackageLocation location);
}
