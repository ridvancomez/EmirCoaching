using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Identity;

namespace EmirCoaching.DataAccess.Repositories.Abstract;

public interface ILoginAttemptRepository : IRepository<LoginAttempt>
{
    // Admin listesi: kullanıcıyla birlikte, yeniden eskiye
    Task<PagedList<LoginAttempt>> GetPagedAsync(LoginAttemptResult? result, int page, int size);

    // Saklama süresi dolanları kalıcı siler (soft delete değil); silinen sayısı
    Task<int> DeletePermanentlyBeforeAsync(DateTime createdBefore);
}
