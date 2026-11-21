using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmirCoaching.DataAccess.Configurations;

public class DietFormSubmissionConfiguration : IEntityTypeConfiguration<DietFormSubmission>
{
    public void Configure(EntityTypeBuilder<DietFormSubmission> builder)
    {
        builder.Property(x => x.FullName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(256).IsRequired();
        builder.Property(x => x.PhoneNumber).HasMaxLength(20).IsRequired();
        builder.Property(x => x.HeightCm).HasPrecision(5, 1);
        builder.Property(x => x.CurrentWeight).HasPrecision(5, 1);
        builder.Property(x => x.TargetWeight).HasPrecision(5, 1);
        builder.Property(x => x.ClientFormUrl).HasMaxLength(500);

        // Paket soft delete olur, satır fiziksel olarak kalır; Restrict yanlışlıkla kalıcı silmeyi engeller
        builder.HasOne(x => x.Package)
            .WithMany()
            .HasForeignKey(x => x.PackageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.Email);
        builder.HasIndex(x => x.PhoneNumber);

        builder.HasMany(x => x.ConsentRecords)
            .WithOne(x => x.DietFormSubmission)
            .HasForeignKey(x => x.DietFormSubmissionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
