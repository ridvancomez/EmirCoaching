using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;

namespace EmirCoaching.Business.Services.Abstract;

public interface INewsletterSubscriberService
{
    // Gizlilik onayı zorunlu; gönderilen her onay (verilen/verilmeyen) kaydedilir
    Task<Result> SubscribeAsync(SubscribeNewsletterRequest request, RequestInfo requestInfo);

    // Admin
    Task<Result<PagedResult<NewsletterSubscriberResponse>>> GetPagedAsync(int page, int size);
    Task<Result> DeleteAsync(int id); // kalıcı silme (KVKK)
}
