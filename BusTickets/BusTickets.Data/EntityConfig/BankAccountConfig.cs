namespace BusTickets.Data.EntityConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    internal class BankAccountConfig : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.HasKey(b => b.BankAccountId);

            builder
                .HasOne(c => c.Customer)
                .WithOne(b => b.BankAccount)
                .HasForeignKey<Customer>(b => b.BankAccountId);
        }
    }
}
