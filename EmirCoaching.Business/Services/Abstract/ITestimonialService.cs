using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;

namespace EmirCoaching.Business.Services.Abstract;

public interface ITestimonialService : ICrudService<TestimonialResponse, CreateTestimonialRequest, UpdateTestimonialRequest>
{
    // Admin: pasifler dahil, packageId verilirse yalnızca o paketin yorumları.
    // Sitede yorumlar paket detayıyla gelir (GET /packages/{slug}).
    Task<Result<IReadOnlyList<TestimonialResponse>>> GetAllAsync(int? packageId);

    // Site: ana sayfa yorum carousel'i
    Task<Result<IReadOnlyList<TestimonialResponse>>> GetHomeAsync();
}
