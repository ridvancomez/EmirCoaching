using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Extensions;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public class DietFormSubmissionRepository(AppDbContext context)
    : PersonalDataRepository<DietFormSubmission>(context), IDietFormSubmissionRepository
{
    protected override IQueryable<ConsentRecord> ConsentsOf(int id) =>
        Context.ConsentRecords.Where(c => c.DietFormSubmissionId == id);

    public Task<PagedList<DietFormSubmission>> GetPagedAsync(DietFormStatus? status, int page, int size) =>
        Set.AsNoTracking()
            .WhereIf(status is not null, x => x.Status == status)
            .NewestFirst()
            .ToPagedListAsync(page, size);

    public Task<bool> HasPendingAsync(string email, string phoneNumber) =>
        Set.AnyAsync(x => x.Status == DietFormStatus.Yeni && (x.Email == email || x.PhoneNumber == phoneNumber));
}
