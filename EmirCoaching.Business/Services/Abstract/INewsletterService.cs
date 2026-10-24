using EmirCoaching.Business.Common;
using EmirCoaching.Entities.Common;

namespace EmirCoaching.Business.Services.Abstract;

public interface INewsletterService
{
    // Gizlilik onayı zorunlu; gönderilen her onay (verilen/verilmeyen) kaydedilir
    Task<Result> SubscribeAsync(string email, Dictionary<ConsentType, bool> consents, RequestInfo requestInfo);
}
