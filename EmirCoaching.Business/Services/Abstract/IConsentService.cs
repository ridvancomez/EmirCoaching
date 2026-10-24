using EmirCoaching.Entities.Entities;

namespace EmirCoaching.Business.Services.Abstract;

// İç kullanım: form servisleri çağırır. Kaydı UnitOfWork'e ekler, SaveChanges'i çağıran servis yapar.
public interface IConsentService
{
    Task RecordAsync(ConsentRecord record);
}
