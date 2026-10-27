using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;

namespace EmirCoaching.DataAccess.Repositories.Abstract;

public interface IDietFormSubmissionRepository : IPersonalDataRepository<DietFormSubmission>
{
    Task<PagedList<DietFormSubmission>> GetPagedAsync(DietFormStatus? status, int page, int size);

    // Aynı e-posta veya telefonla değerlendirilmeyi bekleyen (Yeni) başvuru var mı
    Task<bool> HasPendingAsync(string email, string phoneNumber);
}
