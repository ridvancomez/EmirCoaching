using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmirCoaching.DataAccess.Configurations;

public class DietProgramConfiguration : IEntityTypeConfiguration<DietProgram>
{
    public void Configure(EntityTypeBuilder<DietProgram> builder)
    {
        builder.Property(x => x.Title).HasMaxLength(150).IsRequired();
        builder.Property(x => x.DietitianTitle).HasMaxLength(50).IsRequired();
        builder.Property(x => x.DietitianFullName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.DietitianPhotoPath).HasMaxLength(300);
    }
}
