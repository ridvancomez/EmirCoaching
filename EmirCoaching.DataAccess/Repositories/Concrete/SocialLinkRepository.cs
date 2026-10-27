using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Extensions;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public class SocialLinkRepository(AppDbContext context)
    : ActiveSortedRepository<SocialLink>(context), ISocialLinkRepository
{
    public async Task<SocialLink?> GetActiveByPlatformAsync(SocialPlatform platform) =>
        await Set.AsNoTracking().ActiveOnly().FirstOrDefaultAsync(x => x.Platform == platform);

    public Task<bool> PlatformExistsAsync(SocialPlatform platform, int? excludeId = null) =>
        Set.AnyAsync(x => x.Platform == platform && x.Id != excludeId);
}
