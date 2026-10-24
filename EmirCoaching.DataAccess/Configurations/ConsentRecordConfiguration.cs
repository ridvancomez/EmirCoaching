using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmirCoaching.DataAccess.Configurations;

public class ConsentRecordConfiguration : IEntityTypeConfiguration<ConsentRecord>
{
    public void Configure(EntityTypeBuilder<ConsentRecord> builder)
    {
        builder.Property(x => x.IpAddress).HasMaxLength(45).IsRequired(); // IPv6 dahil
        builder.Property(x => x.UserAgent).HasMaxLength(512).IsRequired();
        builder.Property(x => x.IysStatus).HasMaxLength(50);

        // Tam bir sahip FK dolu olmalı
        builder.ToTable(t => t.HasCheckConstraint(
            "CK_ConsentRecords_SingleOwner",
            "(CASE WHEN [ContactMessageId] IS NULL THEN 0 ELSE 1 END" +
            " + CASE WHEN [DietFormSubmissionId] IS NULL THEN 0 ELSE 1 END" +
            " + CASE WHEN [NewsletterSubscriberId] IS NULL THEN 0 ELSE 1 END) = 1"));
    }
}
