using EmirCoaching.Entities.Common;

namespace EmirCoaching.DataAccess.Repositories.Abstract;

// Kişisel veri taşıyan form kayıtları (iletişim, diyet başvurusu, bülten aboneliği)
public interface IPersonalDataRepository<T> : IRepository<T> where T : BaseEntity
{
    // KVKK: soft delete değil; kayıt, bağlı onay kayıtlarıyla birlikte veritabanından tamamen silinir.
    // İki ayrı DELETE çalışır; çağıran transaction açar. Kayıt yoksa false.
    Task<bool> DeletePermanentlyAsync(int id);
}
