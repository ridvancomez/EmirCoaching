using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmirCoaching.DataAccess.Configurations;

public class DietFormSubmissionConfiguration : IEntityTypeConfiguration<DietFormSubmission>
{
    public void Configure(EntityTypeBuilder<DietFormSubmission> builder)
    {
        builder.Property(x => x.FullName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.PhoneNumber).HasMaxLength(20).IsRequired();
        builder.Property(x => x.HeightCm).HasPrecision(5, 1);
        builder.Property(x => x.CurrentWeight).HasPrecision(5, 1);
        builder.Property(x => x.TargetWeight).HasPrecision(5, 1);
        builder.Property(x => x.WaterLiters).HasPrecision(4, 1);
        builder.Property(x => x.Allergies).HasMaxLength(500);
        builder.Property(x => x.AvoidedFoods).HasMaxLength(500);
        builder.Property(x => x.Supplements).HasMaxLength(500);

        builder.HasIndex(x => x.Status);

        builder.HasMany(x => x.ConsentRecords)
            .WithOne(x => x.DietFormSubmission)
            .HasForeignKey(x => x.DietFormSubmissionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
