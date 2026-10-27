using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public abstract class PersonalDataRepository<T>(AppDbContext context) : Repository<T>(context), IPersonalDataRepository<T>
    where T : BaseEntity
{
    // Kayda bağlı onay kayıtları
    protected abstract IQueryable<ConsentRecord> ConsentsOf(int id);

    // ExecuteDelete SaveChanges'i atlar, bu yüzden soft delete'e dönüşmez. Soft delete'li kayıtlar da silinir.
    public async Task<bool> DeletePermanentlyAsync(int id)
    {
        await ConsentsOf(id).IgnoreQueryFilters().ExecuteDeleteAsync();
        return await Set.IgnoreQueryFilters().Where(x => x.Id == id).ExecuteDeleteAsync() > 0;
    }
}
