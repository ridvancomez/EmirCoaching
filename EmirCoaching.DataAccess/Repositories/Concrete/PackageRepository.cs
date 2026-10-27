using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Extensions;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public class PackageRepository(AppDbContext context) : SlugRepository<Package>(context), IPackageRepository
{
    public async Task<IReadOnlyList<Package>> GetActiveListAsync() =>
        await Set.AsNoTracking()
            .ActiveOnly()
            .Include(x => x.Highlights.OrderBy(h => h.SortOrder))
            .OrderBy(x => x.Location).ThenBy(x => x.SortOrder)
            .ToListAsync();

    public async Task<IReadOnlyList<Package>> GetOrderedListAsync() =>
        await Set.AsNoTracking()
            .Include(x => x.Highlights.OrderBy(h => h.SortOrder))
            .OrderBy(x => x.Location).ThenBy(x => x.SortOrder)
            .ToListAsync();

    public async Task<Package?> GetBySlugWithDetailsAsync(string slug) =>
        await OrderedDetails()
            .Include(x => x.Testimonials.Where(t => t.IsActive).OrderBy(t => t.SortOrder))
            .FirstOrDefaultAsync(x => x.Slug == slug && x.IsActive);

    public async Task<Package?> GetDetailByIdAsync(int id) =>
        await OrderedDetails()
            .Include(x => x.Testimonials.OrderBy(t => t.SortOrder))
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<Package?> GetByIdWithDetailsAsync(int id) =>
        await Set.AsSplitQuery()
            .Include(x => x.Modules).ThenInclude(m => m.Items)
            .Include(x => x.Highlights)
            .Include(x => x.Testimonials)
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<Package?> GetFeaturedAsync(PackageLocation location) =>
        await Set.FirstOrDefaultAsync(x => x.Location == location && x.IsFeatured);

    // Detay sayfası ve admin formu: alt kayıtlar sitedeki sırayla
    private IQueryable<Package> OrderedDetails() =>
        Set.AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Modules.OrderBy(m => m.SortOrder))
                .ThenInclude(m => m.Items.OrderBy(i => i.SortOrder))
            .Include(x => x.Highlights.OrderBy(h => h.SortOrder));
}
