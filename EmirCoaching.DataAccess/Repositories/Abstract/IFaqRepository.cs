using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;

namespace EmirCoaching.DataAccess.Repositories.Abstract;

public interface IFaqRepository : ISortedRepository<Faq>
{
    Task<IReadOnlyList<Faq>> GetByGroupAsync(FaqGroup group);
}
