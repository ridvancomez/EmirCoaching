using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmirCoaching.DataAccess.Configurations;

public class PageContentConfiguration : IEntityTypeConfiguration<PageContent>
{
    public void Configure(EntityTypeBuilder<PageContent> builder)
    {
        builder.Property(x => x.PageKey).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Title).HasMaxLength(150).IsRequired();
        builder.Property(x => x.HighlightText).HasMaxLength(50);
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.Property(x => x.BackgroundImagePath).HasMaxLength(300);
        builder.Property(x => x.ButtonUrl).HasMaxLength(300);

        builder.HasIndex(x => x.PageKey).IsUnique().HasFilter("[IsDeleted] = 0");
    }
}
