namespace P03_FootballBetting.Data.Configuration
{
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder
                .HasKey(p => p.PositionId);

            builder
                .Property(p => p.Name)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(20);
        }
    }
}
