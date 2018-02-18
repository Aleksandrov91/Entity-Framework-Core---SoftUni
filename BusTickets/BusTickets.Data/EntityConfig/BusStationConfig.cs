namespace BusTickets.Data.EntityConfig
{    
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    internal class BusStationConfig : IEntityTypeConfiguration<BusStation>
    {
        public void Configure(EntityTypeBuilder<BusStation> builder)
        {
            builder.HasKey(bs => bs.StationId);

            builder
                .HasOne(t => t.Town)
                .WithMany(s => s.BusStations);
        }
    }
}
