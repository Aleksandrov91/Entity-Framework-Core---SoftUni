namespace BusTickets.Data.EntityConfig
{
    using BusTickets.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class CompanyConfig : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasKey(c => c.CompanyId);

            builder.Property(c => c.Name)
                .HasMaxLength(20)
                .IsRequired()
                .IsUnicode();

            builder
                .HasMany(r => r.Reviews)
                .WithOne(c => c.Company)
                .HasForeignKey(r => r.CompanyId);
        }
    }
}
