using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmirCoaching.DataAccess.Configurations;

public class TransformationTypeConfiguration : IEntityTypeConfiguration<TransformationType>
{
    public void Configure(EntityTypeBuilder<TransformationType> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Slug).HasMaxLength(120).IsRequired();

        builder.HasIndex(x => x.Slug).IsUnique().HasFilter("[IsDeleted] = 0");

        builder.HasMany(x => x.Transformations)
            .WithOne(x => x.TransformationType)
            .HasForeignKey(x => x.TransformationTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
