using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Extensions;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public class SocialPostRepository(AppDbContext context)
    : ActiveSortedRepository<SocialPost>(context), ISocialPostRepository
{
    public async Task<IReadOnlyList<SocialPost>> GetActiveByPlatformAsync(SocialPlatform platform, int count) =>
        await Set.AsNoTracking()
            .ActiveOnly()
            .Where(x => x.Platform == platform)
            .OrderBy(x => x.SortOrder)
            .ThenByDescending(x => x.PostedAt)
            .Take(count)
            .ToListAsync();

    public async Task<List<SocialPost>> GetByExternalIdsAsync(SocialPlatform platform, IReadOnlyCollection<string> externalIds) =>
        await Set.Where(x => x.Platform == platform && x.ExternalId != null && externalIds.Contains(x.ExternalId))
            .ToListAsync();
}
