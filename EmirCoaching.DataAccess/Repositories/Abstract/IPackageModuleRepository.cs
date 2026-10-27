using EmirCoaching.Entities.Entities;

namespace EmirCoaching.DataAccess.Repositories.Abstract;

// Paket güncellemesinde eski modüller silinir; kalemleri cascade ile gider
public interface IPackageModuleRepository : IRepository<PackageModule>;
