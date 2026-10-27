using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Extensions;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public class DietProgramRepository(AppDbContext context)
    : Repository<DietProgram>(context), IDietProgramRepository
{
    public async Task<DietProgram?> GetActiveAsync() =>
        await Set.AsNoTracking().ActiveOnly().FirstOrDefaultAsync();
}
