using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public class ContactMessageRepository(AppDbContext context)
    : Repository<ContactMessage>(context), IContactMessageRepository
{
    public Task<PagedList<ContactMessage>> GetPagedAsync(ContactMessageStatus? status, int page, int size)
    {
        var query = Set.AsNoTracking().Include(x => x.Package).AsQueryable();

        if (status is not null)
            query = query.Where(x => x.Status == status);

        return ToPagedListAsync(query.OrderByDescending(x => x.CreatedAt), page, size);
    }
}
