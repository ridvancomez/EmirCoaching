using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public class AppUserRepository(AppDbContext context) : IAppUserRepository
{
    public Task<AppUser?> GetCoachAsync() =>
        context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.IsCoach);
}
