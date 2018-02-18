namespace Stations.Data.EntityConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    internal class TicketConfig : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Price)
                .IsRequired();

            builder.Property(t => t.SeatingPlace)
                .HasMaxLength(8)
                .IsRequired()
                .IsUnicode();

            builder.HasOne(t => t.CustomerCard)
                .WithMany(cc => cc.BoughtTickets)
                .HasForeignKey(t => t.CustomerCardId);
        }
    }
}
