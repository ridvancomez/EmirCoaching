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

    // Toplu kalıcı silme, ya hepsi ya hiçbiri: kayıtlardan biri yoksa hiçbiri silinmez, false döner
    public static async Task<bool> DeletePermanentlyAsync<T>(this IUnitOfWork unitOfWork, IPersonalDataRepository<T> repository,
        IReadOnlyCollection<int> ids) where T : BaseEntity
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            foreach (var id in ids.Distinct())
            {
                if (await repository.DeletePermanentlyAsync(id))
                    continue;

                await unitOfWork.RollbackAsync();
                return false;
            }

            await unitOfWork.CommitAsync();
            return true;
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }
}
