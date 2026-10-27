using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmirCoaching.DataAccess.Configurations;

public class SiteSettingConfiguration : IEntityTypeConfiguration<SiteSetting>
{
    public void Configure(EntityTypeBuilder<SiteSetting> builder)
    {
        builder.Property(x => x.Phone).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(256).IsRequired();
        builder.Property(x => x.LegalEmail).HasMaxLength(256).IsRequired();
        builder.Property(x => x.Address).HasMaxLength(300).IsRequired();
        builder.Property(x => x.WorkingDays1).HasMaxLength(50).IsRequired();
        builder.Property(x => x.WorkingHours1).HasMaxLength(50).IsRequired();
        builder.Property(x => x.WorkingDays2).HasMaxLength(50).IsRequired();
        builder.Property(x => x.WorkingHours2).HasMaxLength(50).IsRequired();
    }
}
