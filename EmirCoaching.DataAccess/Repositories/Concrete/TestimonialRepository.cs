using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Extensions;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public class TestimonialRepository(AppDbContext context)
    : ActiveSortedRepository<Testimonial>(context), ITestimonialRepository
{
    public async Task<IReadOnlyList<Testimonial>> GetOrderedAsync(int? packageId) =>
        await Set.AsNoTracking()
            .Include(x => x.Package)
            .WhereIf(packageId.HasValue, x => x.PackageId == packageId)
            .Ordered()
            .ToListAsync();

    public async Task<IReadOnlyList<Testimonial>> GetHomeAsync() =>
        await Set.AsNoTracking()
            .Include(x => x.Package)
            .ActiveOnly()
            .Where(x => x.ShowOnHome && x.Package.IsActive)
            .Ordered()
            .ToListAsync();
}
