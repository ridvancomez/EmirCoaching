using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Entities;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public class PackageHighlightRepository(AppDbContext context) : Repository<PackageHighlight>(context), IPackageHighlightRepository;
