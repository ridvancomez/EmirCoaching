using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmirCoaching.DataAccess.Configurations;

public class TransformationMetricConfiguration : IEntityTypeConfiguration<TransformationMetric>
{
    public void Configure(EntityTypeBuilder<TransformationMetric> builder)
    {
        builder.Property(x => x.Label).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Value).HasMaxLength(30).IsRequired();
    }
}
