using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public class PackageRepository(AppDbContext context) : Repository<Package>(context), IPackageRepository
{
    public async Task<IReadOnlyList<Package>> GetActiveListAsync() =>
        await Set.AsNoTracking()
            .Where(x => x.IsActive)
            .Include(x => x.Highlights.OrderBy(h => h.SortOrder))
            .OrderBy(x => x.Location).ThenBy(x => x.SortOrder)
            .ToListAsync();

    public async Task<Package?> GetBySlugWithDetailsAsync(string slug) =>
        await Set.AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Modules.OrderBy(m => m.SortOrder))
                .ThenInclude(m => m.Items.OrderBy(i => i.SortOrder))
            .Include(x => x.Highlights.OrderBy(h => h.SortOrder))
            .Include(x => x.Testimonials.Where(t => t.IsActive).OrderBy(t => t.SortOrder))
            .FirstOrDefaultAsync(x => x.Slug == slug && x.IsActive);

    public async Task<Package?> GetByIdWithDetailsAsync(int id) =>
        await Set.AsSplitQuery()
            .Include(x => x.Modules).ThenInclude(m => m.Items)
            .Include(x => x.Highlights)
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<bool> SlugExistsAsync(string slug, int? excludeId = null) =>
        await Set.AnyAsync(x => x.Slug == slug && x.Id != excludeId);
}
