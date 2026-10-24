using EmirCoaching.Entities.Common;

namespace EmirCoaching.DataAccess.Repositories.Abstract;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity); // AppDbContext soft delete'e çevirir
}
