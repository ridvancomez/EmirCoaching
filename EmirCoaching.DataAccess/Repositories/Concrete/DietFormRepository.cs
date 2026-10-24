using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public class DietFormRepository(AppDbContext context)
    : Repository<DietFormSubmission>(context), IDietFormRepository
{
    public Task<PagedList<DietFormSubmission>> GetPagedAsync(DietFormStatus? status, int page, int size)
    {
        var query = Set.AsNoTracking();

        if (status is not null)
            query = query.Where(x => x.Status == status);

        return ToPagedListAsync(query.OrderByDescending(x => x.CreatedAt), page, size);
    }
}
