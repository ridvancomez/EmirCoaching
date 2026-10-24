using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmirCoaching.DataAccess.Configurations;

public class PackageModuleItemConfiguration : IEntityTypeConfiguration<PackageModuleItem>
{
    public void Configure(EntityTypeBuilder<PackageModuleItem> builder)
    {
        builder.Property(x => x.Text).HasMaxLength(200).IsRequired();
        builder.Property(x => x.LockedLabel).HasMaxLength(100);
    }
}
