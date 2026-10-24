using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmirCoaching.DataAccess.Configurations;

public class SiteStatConfiguration : IEntityTypeConfiguration<SiteStat>
{
    public void Configure(EntityTypeBuilder<SiteStat> builder)
    {
        builder.Property(x => x.Label).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Value).HasMaxLength(30).IsRequired();
        builder.Property(x => x.Suffix).HasMaxLength(20);
    }
}
