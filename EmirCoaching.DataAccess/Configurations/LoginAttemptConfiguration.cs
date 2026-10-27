using EmirCoaching.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmirCoaching.DataAccess.Configurations;

public class LoginAttemptConfiguration : IEntityTypeConfiguration<LoginAttempt>
{
    public void Configure(EntityTypeBuilder<LoginAttempt> builder)
    {
        builder.Property(x => x.IpAddress).HasMaxLength(45).IsRequired(); // IPv6 dahil
        builder.Property(x => x.UserAgent).HasMaxLength(512).IsRequired();

        builder.HasIndex(x => x.CreatedAt); // liste sırası ve eski kayıt temizliği

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
