using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;

namespace EmirCoaching.DataAccess.Repositories.Abstract;

public interface ILegalDocumentRepository : IRepository<LegalDocument>
{
    Task<LegalDocument?> GetCurrentAsync(LegalDocumentType type);

    // Silinmiş sürümler dahil en yüksek sürüm numarası (yoksa 0)
    Task<int> GetLatestVersionAsync(LegalDocumentType type);
}
