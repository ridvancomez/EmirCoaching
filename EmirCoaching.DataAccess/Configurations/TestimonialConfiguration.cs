using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmirCoaching.DataAccess.Configurations;

public class TestimonialConfiguration : IEntityTypeConfiguration<Testimonial>
{
    public void Configure(EntityTypeBuilder<Testimonial> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Text).HasMaxLength(1000).IsRequired();
        builder.Property(x => x.Rating).HasPrecision(2, 1);

        // 0–5 arası, yalnızca tam ve buçuklu değerler
        builder.ToTable(t => t.HasCheckConstraint(
            "CK_Testimonials_Rating",
            "[Rating] >= 0 AND [Rating] <= 5 AND [Rating] * 2 = FLOOR([Rating] * 2)"));
    }
}
