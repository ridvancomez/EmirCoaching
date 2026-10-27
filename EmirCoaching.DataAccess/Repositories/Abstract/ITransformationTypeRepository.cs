using EmirCoaching.Entities.Entities;

namespace EmirCoaching.DataAccess.Repositories.Abstract;

// Değişimler sayfasındaki filtre sekmeleri
public interface ITransformationTypeRepository : ISortedRepository<TransformationType>, ISlugRepository<TransformationType>;
