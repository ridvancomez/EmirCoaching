using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmirCoaching.DataAccess.Configurations;

public class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
{
    public void Configure(EntityTypeBuilder<Certificate> builder)
    {
        builder.Property(x => x.Title).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Issuer).HasMaxLength(50).IsRequired();
        builder.Property(x => x.IssuerFullName).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Detail).HasMaxLength(100);
        builder.Property(x => x.LogoPath).HasMaxLength(300).IsRequired();
    }
}
