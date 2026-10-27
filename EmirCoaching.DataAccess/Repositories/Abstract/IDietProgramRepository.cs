using EmirCoaching.Entities.Entities;

namespace EmirCoaching.DataAccess.Repositories.Abstract;

public interface IDietProgramRepository : IRepository<DietProgram>
{
    // Diyet planı detay sayfası: aktif program
    Task<DietProgram?> GetActiveAsync();
}
