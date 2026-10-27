using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Extensions;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public class ContactMessageRepository(AppDbContext context)
    : PersonalDataRepository<ContactMessage>(context), IContactMessageRepository
{
    protected override IQueryable<ConsentRecord> ConsentsOf(int id) =>
        Context.ConsentRecords.Where(c => c.ContactMessageId == id);

    public Task<PagedList<ContactMessage>> GetPagedAsync(ContactMessageStatus? status, int page, int size) =>
        Set.AsNoTracking()
            .Include(x => x.Package)
            .WhereIf(status is not null, x => x.Status == status)
            .NewestFirst()
            .ToPagedListAsync(page, size);
}
