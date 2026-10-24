using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmirCoaching.DataAccess.Configurations;

public class SiteSettingConfiguration : IEntityTypeConfiguration<SiteSetting>
{
    public void Configure(EntityTypeBuilder<SiteSetting> builder)
    {
        builder.Property(x => x.WhatsApp).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Phone).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(256).IsRequired();
        builder.Property(x => x.Address).HasMaxLength(300).IsRequired();
        builder.Property(x => x.Instagram).HasMaxLength(300);
        builder.Property(x => x.Facebook).HasMaxLength(300);
        builder.Property(x => x.LinkedIn).HasMaxLength(300);
        builder.Property(x => x.Twitter).HasMaxLength(300);
        builder.Property(x => x.WorkingHours).HasMaxLength(300).IsRequired();
    }
}
