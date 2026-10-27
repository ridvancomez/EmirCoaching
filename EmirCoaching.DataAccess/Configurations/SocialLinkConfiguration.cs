using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmirCoaching.DataAccess.Configurations;

public class SocialLinkConfiguration : IEntityTypeConfiguration<SocialLink>
{
    public void Configure(EntityTypeBuilder<SocialLink> builder)
    {
        builder.Property(x => x.Url).HasMaxLength(300).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(200).IsRequired();

        // Her platform için tek kayıt
        builder.HasIndex(x => x.Platform).IsUnique().HasFilter("[IsDeleted] = 0");
    }
}
