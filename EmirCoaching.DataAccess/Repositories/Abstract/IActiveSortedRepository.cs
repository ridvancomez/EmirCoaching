using EmirCoaching.Entities.Common;

namespace EmirCoaching.DataAccess.Repositories.Abstract;

public interface IActiveSortedRepository<T> : ISortedRepository<T> where T : BaseEntity, ISortable, IActivatable
{
    // Site listesi: yalnızca aktifler, SortOrder'a göre
    Task<IReadOnlyList<T>> GetActiveOrderedAsync();
}
