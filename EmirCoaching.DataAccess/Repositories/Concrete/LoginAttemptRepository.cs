using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Extensions;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public class LoginAttemptRepository(AppDbContext context)
    : Repository<LoginAttempt>(context), ILoginAttemptRepository
{
    public Task<PagedList<LoginAttempt>> GetPagedAsync(LoginAttemptResult? result, int page, int size) =>
        Set.AsNoTracking()
            .Include(x => x.User)
            .WhereIf(result is not null, x => x.Result == result)
            .NewestFirst()
            .ToPagedListAsync(page, size);

    public Task<int> DeletePermanentlyBeforeAsync(DateTime createdBefore) =>
        Set.IgnoreQueryFilters().Where(x => x.CreatedAt < createdBefore).ExecuteDeleteAsync();
}
