using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public abstract class Repository<T>(AppDbContext context) : IRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext Context = context;
    protected readonly DbSet<T> Set = context.Set<T>();

    public async Task<T?> GetByIdAsync(int id) =>
        await Set.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IReadOnlyList<T>> GetAllAsync() =>
        await Set.AsNoTracking().ToListAsync();

    public async Task AddAsync(T entity) =>
        await Set.AddAsync(entity);

    public void Update(T entity) =>
        Set.Update(entity);

    public void Remove(T entity) =>
        Set.Remove(entity);
}
