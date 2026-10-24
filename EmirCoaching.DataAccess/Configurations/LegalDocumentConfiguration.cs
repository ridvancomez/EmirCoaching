using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmirCoaching.DataAccess.Configurations;

public class LegalDocumentConfiguration : IEntityTypeConfiguration<LegalDocument>
{
    public void Configure(EntityTypeBuilder<LegalDocument> builder)
    {
        builder.Property(x => x.Title).HasMaxLength(150).IsRequired();
        builder.Property(x => x.FilePath).HasMaxLength(300);

        builder.HasIndex(x => new { x.Type, x.Version }).IsUnique();

        // Her tür için tek "güncel" sürüm
        builder.HasIndex(x => x.Type)
            .IsUnique()
            .HasFilter("[IsCurrent] = 1 AND [IsDeleted] = 0")
            .HasDatabaseName("IX_LegalDocuments_Type_Current");

        builder.HasMany(x => x.ConsentRecords)
            .WithOne(x => x.LegalDocument)
            .HasForeignKey(x => x.LegalDocumentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
