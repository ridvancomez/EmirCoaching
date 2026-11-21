using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Entities;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public class CertificateRepository(AppDbContext context)
    : ActiveSortedRepository<Certificate>(context), ICertificateRepository;
