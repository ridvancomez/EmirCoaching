using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmirCoaching.DataAccess.Configurations;

public class NewsletterSubscriberConfiguration : IEntityTypeConfiguration<NewsletterSubscriber>
{
    public void Configure(EntityTypeBuilder<NewsletterSubscriber> builder)
    {
        builder.Property(x => x.Email).HasMaxLength(256).IsRequired();

        builder.Property(x => x.IpAddress).HasMaxLength(45).IsRequired(); // IPv6 dahil

        builder.HasIndex(x => x.Email).IsUnique().HasFilter("[IsDeleted] = 0");
        builder.HasIndex(x => new { x.IpAddress, x.CreatedAt });

        builder.HasMany(x => x.ConsentRecords)
            .WithOne(x => x.NewsletterSubscriber)
            .HasForeignKey(x => x.NewsletterSubscriberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
