using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Entities.Common;

namespace EmirCoaching.Business.Services.Abstract;

public interface IContactService
{
    Task<Result> SubmitAsync(CreateContactMessageDto dto, RequestInfo requestInfo);

    // Admin
    Task<PagedResult<ContactMessageDto>> GetPagedAsync(ContactMessageStatus? status, int page, int size);
    Task<Result> UpdateStatusAsync(int id, ContactMessageStatus newStatus);
}
