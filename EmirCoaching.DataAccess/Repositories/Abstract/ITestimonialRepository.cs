using EmirCoaching.Entities.Entities;

namespace EmirCoaching.DataAccess.Repositories.Abstract;

public interface ITestimonialRepository : IActiveSortedRepository<Testimonial>
{
    // Admin listesi: pasifler dahil, packageId verilirse yalnızca o paketin yorumları
    Task<IReadOnlyList<Testimonial>> GetOrderedAsync(int? packageId);
}
