using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;

namespace EmirCoaching.DataAccess.Repositories.Abstract;

public interface IContactMessageRepository : IPersonalDataRepository<ContactMessage>
{
    Task<PagedList<ContactMessage>> GetPagedAsync(ContactMessageStatus? status, int page, int size);
}
