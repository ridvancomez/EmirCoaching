using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmirCoaching.DataAccess.Configurations;

public class NewsletterSubscriberConfiguration : IEntityTypeConfiguration<NewsletterSubscriber>
{
    public void Configure(EntityTypeBuilder<NewsletterSubscriber> builder)
    {
        builder.Property(x => x.Email).HasMaxLength(256).IsRequired();

        builder.HasIndex(x => x.Email).IsUnique().HasFilter("[IsDeleted] = 0");

        builder.HasMany(x => x.ConsentRecords)
            .WithOne(x => x.NewsletterSubscriber)
            .HasForeignKey(x => x.NewsletterSubscriberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
