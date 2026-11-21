using EmirCoaching.Entities.Identity;

namespace EmirCoaching.DataAccess.Repositories.Abstract;

// Salt okunur: kullanıcı oluşturma/güncelleme ASP.NET Identity'nin UserManager'ı üzerinden yapılır
public interface IAppUserRepository
{
    // Sitedeki koç bölümünde gösterilen kullanıcı (IsCoach); yoksa null
    Task<AppUser?> GetCoachAsync();
}
