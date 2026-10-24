using System.Linq.Expressions;
using EmirCoaching.DataAccess.Seed;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using EmirCoaching.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.DataAccess.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<AppUser, IdentityRole<int>, int>(options)
{
    public DbSet<Package> Packages { get; set; }
    public DbSet<PackageModule> PackageModules { get; set; }
    public DbSet<PackageModuleItem> PackageModuleItems { get; set; }
    public DbSet<PackageHighlight> PackageHighlights { get; set; }
    public DbSet<Testimonial> Testimonials { get; set; }
    public DbSet<BlogCategory> BlogCategories { get; set; }
    public DbSet<BlogPost> BlogPosts { get; set; }
    public DbSet<Transformation> Transformations { get; set; }
    public DbSet<TransformationMetric> TransformationMetrics { get; set; }
    public DbSet<Faq> Faqs { get; set; }
    public DbSet<SiteStat> SiteStats { get; set; }
    public DbSet<SiteSetting> SiteSettings { get; set; }
    public DbSet<LegalDocument> LegalDocuments { get; set; }
    public DbSet<ContactMessage> ContactMessages { get; set; }
    public DbSet<DietFormSubmission> DietFormSubmissions { get; set; }
    public DbSet<NewsletterSubscriber> NewsletterSubscribers { get; set; }
    public DbSet<ConsentRecord> ConsentRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Soft delete: BaseEntity'den türeyen her entity'ye "IsDeleted == false" filtresi
        foreach (var entityType in builder.Model.GetEntityTypes()
                     .Where(t => typeof(BaseEntity).IsAssignableFrom(t.ClrType) && t.BaseType is null))
        {
            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var body = Expression.Equal(
                Expression.Property(parameter, nameof(BaseEntity.IsDeleted)),
                Expression.Constant(false));
            builder.Entity(entityType.ClrType).HasQueryFilter(Expression.Lambda(body, parameter));
        }

        SeedData.Seed(builder);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplyAuditRules();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ApplyAuditRules();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    // CreatedAt/UpdatedAt doldurur; Remove() çağrılarını soft delete'e çevirir.
    private void ApplyAuditRules()
    {
        ChangeTracker.DetectChanges();
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    if (entry.Entity.CreatedAt == default)
                        entry.Entity.CreatedAt = now;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = now;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = now;
                    break;
            }
        }
    }
}
