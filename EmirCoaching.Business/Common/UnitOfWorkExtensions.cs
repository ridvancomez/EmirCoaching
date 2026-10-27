using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Common;

namespace EmirCoaching.Business.Common;

public static class UnitOfWorkExtensions
{
    // Form kaydını onay kayıtlarıyla birlikte tek transaction'da kalıcı siler; kayıt yoksa false
    public static async Task<bool> DeletePermanentlyAsync<T>(this IUnitOfWork unitOfWork, IPersonalDataRepository<T> repository, int id)
        where T : BaseEntity
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var deleted = await repository.DeletePermanentlyAsync(id);
            await unitOfWork.CommitAsync();
            return deleted;
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }
}
