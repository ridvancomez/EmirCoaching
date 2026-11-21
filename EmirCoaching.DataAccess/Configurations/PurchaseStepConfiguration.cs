using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmirCoaching.DataAccess.Configurations;

public class PurchaseStepConfiguration : IEntityTypeConfiguration<PurchaseStep>
{
    public void Configure(EntityTypeBuilder<PurchaseStep> builder)
    {
        builder.Property(x => x.Title).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(300).IsRequired();
        builder.Property(x => x.TimeLabel).HasMaxLength(50).IsRequired();
        builder.Property(x => x.IconKey).HasMaxLength(50).IsRequired();
    }
}
