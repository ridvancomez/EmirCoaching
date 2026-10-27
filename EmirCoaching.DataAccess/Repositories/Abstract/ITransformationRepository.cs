using EmirCoaching.Entities.Entities;

namespace EmirCoaching.DataAccess.Repositories.Abstract;

public interface ITransformationRepository : IRepository<Transformation>
{
    Task<IReadOnlyList<Transformation>> GetListAsync(string? typeSlug, TransformationSort sort);
    Task<IReadOnlyList<Transformation>> GetForHomeAsync();
    Task<Transformation?> GetWithMetricsAsync(int id);

    // Admin listesi: pasifler dahil, sitedeki varsayılan sırayla (yeniden eskiye)
    Task<IReadOnlyList<Transformation>> GetOrderedListAsync();

    // Tip silinmeden önce: bu tipte (silinmemiş) değişim var mı
    Task<bool> AnyWithTypeAsync(int transformationTypeId);
}
