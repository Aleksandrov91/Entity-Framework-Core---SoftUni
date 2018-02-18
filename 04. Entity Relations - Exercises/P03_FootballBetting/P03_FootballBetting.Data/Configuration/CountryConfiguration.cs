namespace P03_FootballBetting.Data.Configuration
{
    using Data.Models;
    using Microsoft.EntityFrameworkCore;    
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder
                .HasKey(c => c.CountryId);

            builder
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(30);
        }
    }
}
