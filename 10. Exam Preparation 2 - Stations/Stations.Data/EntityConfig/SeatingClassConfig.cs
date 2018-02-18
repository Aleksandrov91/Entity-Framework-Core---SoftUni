namespace Stations.Data.EntityConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    internal class SeatingClassConfig : IEntityTypeConfiguration<SeatingClass>
    {
        public void Configure(EntityTypeBuilder<SeatingClass> builder)
        {
            builder.HasKey(sc => sc.Id);

            builder.HasIndex(sc => sc.Name).IsUnique();

            builder.Property(sc => sc.Name)
                .HasMaxLength(30)
                .IsRequired()
                .IsUnicode();

            builder.Property(sc => sc.Abbreviation)
                .HasColumnType("CHAR(2)")
                .IsRequired()
                .IsUnicode();
        }
    }
}
