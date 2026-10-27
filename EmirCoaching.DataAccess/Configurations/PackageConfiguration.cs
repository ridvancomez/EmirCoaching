using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmirCoaching.DataAccess.Configurations;

public class PackageConfiguration : IEntityTypeConfiguration<Package>
{
    public void Configure(EntityTypeBuilder<Package> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Slug).HasMaxLength(120).IsRequired();
        builder.Property(x => x.Subtitle).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Price).HasPrecision(10, 2);
        builder.Property(x => x.ShortDescription).HasMaxLength(500).IsRequired();
        builder.Property(x => x.PurchaseUrl).HasMaxLength(300).IsRequired();
        builder.Property(x => x.MetaTitle).HasMaxLength(70);
        builder.Property(x => x.MetaDescription).HasMaxLength(170);

        // Silinmiş kaydın slug'ı yeniden kullanılabilsin diye filtreli unique index
        builder.HasIndex(x => x.Slug).IsUnique().HasFilter("[IsDeleted] = 0");

        // Her kategoride (Salonda / Evde) tek "En Çok Tercih Edilen" paket
        builder.HasIndex(x => x.Location)
            .IsUnique()
            .HasFilter("[IsFeatured] = 1 AND [IsDeleted] = 0")
            .HasDatabaseName("IX_Packages_Location_Featured");

        builder.HasMany(x => x.Modules)
            .WithOne(x => x.Package)
            .HasForeignKey(x => x.PackageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Highlights)
            .WithOne(x => x.Package)
            .HasForeignKey(x => x.PackageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Testimonials)
            .WithOne(x => x.Package)
            .HasForeignKey(x => x.PackageId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
