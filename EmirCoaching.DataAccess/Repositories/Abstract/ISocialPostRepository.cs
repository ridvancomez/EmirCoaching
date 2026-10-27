using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;

namespace EmirCoaching.DataAccess.Repositories.Abstract;

public interface ISocialPostRepository : IActiveSortedRepository<SocialPost>
{
    // Sitede gösterilen postlar: SortOrder, sonra en yeni paylaşım (Instagram son 3 post)
    Task<IReadOnlyList<SocialPost>> GetActiveByPlatformAsync(SocialPlatform platform, int count);

    // Instagram senkronu: Meta'dan gelen id'lerden veritabanında olanlar (takip edilen)
    Task<List<SocialPost>> GetByExternalIdsAsync(SocialPlatform platform, IReadOnlyCollection<string> externalIds);
}
