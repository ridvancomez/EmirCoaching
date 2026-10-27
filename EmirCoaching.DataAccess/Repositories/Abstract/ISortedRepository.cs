using EmirCoaching.Entities.Common;

namespace EmirCoaching.DataAccess.Repositories.Abstract;

public interface ISortedRepository<T> : IRepository<T> where T : BaseEntity, ISortable
{
    // Admin listesi: pasifler dahil, SortOrder'a göre
    Task<IReadOnlyList<T>> GetOrderedAsync();
}
