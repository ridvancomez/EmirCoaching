using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmirCoaching.DataAccess.Configurations;

public class TransformationConfiguration : IEntityTypeConfiguration<Transformation>
{
    public void Configure(EntityTypeBuilder<Transformation> builder)
    {
        builder.Property(x => x.ClientName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Quote).HasMaxLength(500).IsRequired();
        builder.Property(x => x.Story).IsRequired();
        builder.Property(x => x.BeforeImagePath).HasMaxLength(300).IsRequired();
        builder.Property(x => x.AfterImagePath).HasMaxLength(300).IsRequired();
        builder.Property(x => x.BeforeSquarePath).HasMaxLength(300);
        builder.Property(x => x.AfterSquarePath).HasMaxLength(300);

        builder.HasMany(x => x.Metrics)
            .WithOne(x => x.Transformation)
            .HasForeignKey(x => x.TransformationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
