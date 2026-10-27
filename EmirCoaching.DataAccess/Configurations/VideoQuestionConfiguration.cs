using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmirCoaching.DataAccess.Configurations;

public class VideoQuestionConfiguration : IEntityTypeConfiguration<VideoQuestion>
{
    public void Configure(EntityTypeBuilder<VideoQuestion> builder)
    {
        builder.Property(x => x.Question).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000).IsRequired();
        builder.Property(x => x.ThumbnailPath).HasMaxLength(300).IsRequired();
        builder.Property(x => x.YouTubeUrl).HasMaxLength(300).IsRequired();
    }
}
