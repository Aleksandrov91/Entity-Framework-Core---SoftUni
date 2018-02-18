namespace P01_StudentSystem.Data.Configuration
{
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder
                .HasKey(r => r.ResourceId);

            builder
                .Property(r => r.Name)
                .HasMaxLength(50)
                .IsUnicode(true)
                .IsRequired(true);

            builder
                .Property(r => r.Url)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsRequired(true);

            builder
                .Property(r => r.ResourceType)
                .IsRequired(true);

            builder
                .HasOne(c => c.Course)
                .WithMany(r => r.Resources);
        }
    }
}
