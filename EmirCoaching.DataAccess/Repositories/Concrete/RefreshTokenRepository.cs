using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public class RefreshTokenRepository(AppDbContext context)
    : Repository<RefreshToken>(context), IRefreshTokenRepository
{
    public async Task<RefreshToken?> GetByHashAsync(string tokenHash) =>
        await Set.Include(x => x.User).FirstOrDefaultAsync(x => x.TokenHash == tokenHash);

    public async Task<IReadOnlyList<RefreshToken>> GetActiveByUserIdAsync(int userId) =>
        await Set.Where(x => x.UserId == userId && x.RevokedAt == null && x.ExpiresAt > DateTime.UtcNow)
            .ToListAsync();

    public Task<int> DeletePermanentlyExpiredBeforeAsync(DateTime expiredBefore) =>
        Set.IgnoreQueryFilters().Where(x => x.ExpiresAt < expiredBefore).ExecuteDeleteAsync();
}
