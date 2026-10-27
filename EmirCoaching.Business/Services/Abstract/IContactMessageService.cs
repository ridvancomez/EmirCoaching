using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Entities.Common;

namespace EmirCoaching.Business.Services.Abstract;

public interface IContactMessageService
{
    Task<Result> SubmitAsync(CreateContactMessageRequest request, RequestInfo requestInfo);

    // Admin
    Task<Result<PagedResult<ContactMessageResponse>>> GetPagedAsync(ContactMessageStatus? status, int page, int size);
    Task<Result> UpdateStatusAsync(int id, ContactMessageStatus newStatus);
    Task<Result> DeleteAsync(int id); // kalıcı silme (KVKK)
}
