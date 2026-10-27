using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmirCoaching.DataAccess.Configurations;

public class SocialPostConfiguration : IEntityTypeConfiguration<SocialPost>
{
    public void Configure(EntityTypeBuilder<SocialPost> builder)
    {
        builder.Property(x => x.ExternalId).HasMaxLength(100);
        builder.Property(x => x.Title).HasMaxLength(150);
        builder.Property(x => x.Caption).HasMaxLength(2200); // Instagram caption sınırı
        builder.Property(x => x.Hashtag).HasMaxLength(100);
        builder.Property(x => x.ImagePath).HasMaxLength(1000).IsRequired(); // Instagram CDN adresleri uzun
        builder.Property(x => x.Url).HasMaxLength(300).IsRequired();

        // Senkronizasyonda aynı Instagram postu iki kez eklenmesin
        builder.HasIndex(x => new { x.Platform, x.ExternalId })
            .IsUnique()
            .HasFilter("[ExternalId] IS NOT NULL AND [IsDeleted] = 0");
    }
}
