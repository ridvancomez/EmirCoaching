using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Entities;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public class ConsentRecordRepository(AppDbContext context) : Repository<ConsentRecord>(context), IConsentRecordRepository;
