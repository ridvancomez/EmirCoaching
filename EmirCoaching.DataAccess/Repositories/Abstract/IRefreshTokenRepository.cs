using EmirCoaching.Entities.Identity;

namespace EmirCoaching.DataAccess.Repositories.Abstract;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    // Kullanıcıyla birlikte (yeni access token için gerekli)
    Task<RefreshToken?> GetByHashAsync(string tokenHash);

    // İptal edilmemiş ve süresi dolmamış token'lar
    Task<IReadOnlyList<RefreshToken>> GetActiveByUserIdAsync(int userId);
    // Süresi bu tarihten önce dolmuş token'ları kalıcı siler; silinen sayısı.
    // İptal edilmiş token süresi dolana kadar tutulur (tekrar kullanımı yakalamak için).
    Task<int> DeletePermanentlyExpiredBeforeAsync(DateTime expiredBefore);
}
