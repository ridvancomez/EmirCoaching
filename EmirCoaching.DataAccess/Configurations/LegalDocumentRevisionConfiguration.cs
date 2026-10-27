using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmirCoaching.DataAccess.Configurations;

public class LegalDocumentRevisionConfiguration : IEntityTypeConfiguration<LegalDocumentRevision>
{
    public void Configure(EntityTypeBuilder<LegalDocumentRevision> builder)
    {
        builder.Property(x => x.Note).HasMaxLength(300).IsRequired();

        builder.HasIndex(x => new { x.Type, x.Version }).IsUnique();

        builder.HasOne(x => x.LegalDocument)
            .WithMany(x => x.Revisions)
            .HasForeignKey(x => x.LegalDocumentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
