using EmirCoaching.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmirCoaching.DataAccess.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(x => x.CoachExperience).HasMaxLength(50);
        builder.Property(x => x.CoachStory).HasMaxLength(2000);
        builder.Property(x => x.CoachVideoUrl).HasMaxLength(300);
        builder.Property(x => x.CoachVideoCoverPath).HasMaxLength(300);

        // Sitede tek koç gösterilir
        builder.HasIndex(x => x.IsCoach).IsUnique().HasFilter("[IsCoach] = 1");

        builder.Property(x => x.FullName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Title).HasMaxLength(100);
        builder.Property(x => x.Bio).HasMaxLength(1000);
        builder.Property(x => x.AvatarPath).HasMaxLength(300);
    }
}
