namespace Stations.Data.EntityConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    internal class StationConfig : IEntityTypeConfiguration<Station>
    {
        public void Configure(EntityTypeBuilder<Station> builder)
        {
            builder.HasKey(s => s.Id);

            builder.HasIndex(s => s.Name).IsUnique();

            builder.Property(s => s.Name)
                .HasMaxLength(50)
                .IsRequired()
                .IsUnicode();

            builder.Property(s => s.Town)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
