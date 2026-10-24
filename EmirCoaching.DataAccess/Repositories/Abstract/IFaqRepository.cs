using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;

namespace EmirCoaching.DataAccess.Repositories.Abstract;

public interface IFaqRepository : IRepository<Faq>
{
    Task<IReadOnlyList<Faq>> GetByGroupAsync(FaqGroup group);
}
