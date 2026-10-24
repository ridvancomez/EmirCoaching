using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;

namespace EmirCoaching.DataAccess.Repositories.Abstract;

public interface IDietFormRepository : IRepository<DietFormSubmission>
{
    Task<PagedList<DietFormSubmission>> GetPagedAsync(DietFormStatus? status, int page, int size);
}
