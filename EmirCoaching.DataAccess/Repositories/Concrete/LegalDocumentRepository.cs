using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public class LegalDocumentRepository(AppDbContext context)
    : Repository<LegalDocument>(context), ILegalDocumentRepository
{
    public async Task<LegalDocument?> GetCurrentAsync(LegalDocumentType type) =>
        await Set.FirstOrDefaultAsync(x => x.Type == type && x.IsCurrent);

    public async Task<int> GetLatestVersionAsync(LegalDocumentType type) =>
        await Set.IgnoreQueryFilters()
            .Where(x => x.Type == type)
            .MaxAsync(x => (int?)x.Version) ?? 0;

    public async Task<IReadOnlyList<LegalDocumentRevision>> GetRevisionsAsync(LegalDocumentType type) =>
        await Context.LegalDocumentRevisions.AsNoTracking()
            .Where(x => x.Type == type)
            .OrderByDescending(x => x.Version)
            .ToListAsync();
}
