using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmirCoaching.DataAccess.Configurations;

public class ContactMessageConfiguration : IEntityTypeConfiguration<ContactMessage>
{
    public void Configure(EntityTypeBuilder<ContactMessage> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Phone).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(256).IsRequired();
        builder.Property(x => x.Location).HasMaxLength(20);
        builder.Property(x => x.Message).HasMaxLength(2000);

        builder.HasIndex(x => x.Status);

        builder.HasOne(x => x.Package)
            .WithMany()
            .HasForeignKey(x => x.PackageId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(x => x.ConsentRecords)
            .WithOne(x => x.ContactMessage)
            .HasForeignKey(x => x.ContactMessageId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
