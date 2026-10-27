using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Entities;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public class TransformationMetricRepository(AppDbContext context) : Repository<TransformationMetric>(context), ITransformationMetricRepository;
