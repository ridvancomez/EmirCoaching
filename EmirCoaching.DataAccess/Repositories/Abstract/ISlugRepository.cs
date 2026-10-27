using EmirCoaching.Entities.Common;

namespace EmirCoaching.DataAccess.Repositories.Abstract;

public interface ISlugRepository<T> : IRepository<T> where T : BaseEntity, ISluggable
{
    // Silinmiş kayıtlar hariç; güncellemede kaydın kendisi excludeId ile dışarıda bırakılır
    Task<bool> SlugExistsAsync(string slug, int? excludeId = null);
}
