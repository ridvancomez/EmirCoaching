using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmirCoaching.DataAccess.Configurations;

public class PackageHighlightConfiguration : IEntityTypeConfiguration<PackageHighlight>
{
    public void Configure(EntityTypeBuilder<PackageHighlight> builder)
    {
        builder.Property(x => x.Text).HasMaxLength(200).IsRequired();
    }
}
