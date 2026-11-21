using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmirCoaching.DataAccess.Configurations;

public class PackageHighlightConfiguration : IEntityTypeConfiguration<PackageHighlight>
{
    public void Configure(EntityTypeBuilder<PackageHighlight> builder)
    {
        builder.Property(x => x.Text).HasMaxLength(200).IsRequired();
        builder.Property(x => x.SubText).HasMaxLength(200);
        builder.Property(x => x.IconKey).HasMaxLength(50).IsRequired();
    }
}
