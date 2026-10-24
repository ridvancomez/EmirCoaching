using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmirCoaching.DataAccess.Configurations;

public class PackageModuleConfiguration : IEntityTypeConfiguration<PackageModule>
{
    public void Configure(EntityTypeBuilder<PackageModule> builder)
    {
        builder.Property(x => x.Title).HasMaxLength(100).IsRequired();
        builder.Property(x => x.IconKey).HasMaxLength(50).IsRequired();

        builder.HasMany(x => x.Items)
            .WithOne(x => x.Module)
            .HasForeignKey(x => x.ModuleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
