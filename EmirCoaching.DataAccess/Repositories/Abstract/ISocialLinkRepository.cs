using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;

namespace EmirCoaching.DataAccess.Repositories.Abstract;

public interface ISocialLinkRepository : IActiveSortedRepository<SocialLink>
{
    // Sitedeki WhatsApp butonları gibi tek platformun aktif linki
    Task<SocialLink?> GetActiveByPlatformAsync(SocialPlatform platform);

    // Her platformdan tek kayıt; güncellemede kaydın kendisi excludeId ile dışarıda bırakılır
    Task<bool> PlatformExistsAsync(SocialPlatform platform, int? excludeId = null);
}
