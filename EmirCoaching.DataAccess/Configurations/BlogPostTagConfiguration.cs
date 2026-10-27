using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmirCoaching.DataAccess.Configurations;

public class BlogPostTagConfiguration : IEntityTypeConfiguration<BlogPostTag>
{
    public void Configure(EntityTypeBuilder<BlogPostTag> builder)
    {
        builder.HasKey(x => new { x.BlogPostId, x.BlogTagId });

        builder.HasOne(x => x.BlogPost)
            .WithMany(x => x.Tags)
            .HasForeignKey(x => x.BlogPostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.BlogTag)
            .WithMany(x => x.Posts)
            .HasForeignKey(x => x.BlogTagId)
            .OnDelete(DeleteBehavior.Cascade);

        // Yazı/etiket soft delete olunca bağlantı da sorgulardan düşsün
        builder.HasQueryFilter(x => !x.BlogPost.IsDeleted && !x.BlogTag.IsDeleted);
    }
}
