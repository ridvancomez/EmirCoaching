using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Entities;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public class BlogCategoryRepository(AppDbContext context) : SlugRepository<BlogCategory>(context), IBlogCategoryRepository;
