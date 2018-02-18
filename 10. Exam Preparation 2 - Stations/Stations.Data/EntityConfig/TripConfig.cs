namespace Stations.Data.EntityConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;
    using Models.Enums;

    internal class TripConfig : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.OriginStationId)
                .IsRequired();

            builder.Property(t => t.DestinationStationId)
                .IsRequired();

            builder.Property(t => t.DepartureTime)
                .IsRequired();

            builder.Property(t => t.ArrivalTime)
                .IsRequired();

            builder.Property(t => t.TrainId)
                .IsRequired();

            builder.Property(t => t.Status)
                .HasDefaultValue(TripStatus.OnTime);

            builder.Property(t => t.TimeDifference)
                .IsRequired(false);

            builder
                .HasOne(t => t.OriginStation)
                .WithMany(s => s.TripsFrom)
                .HasForeignKey(t => t.OriginStationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(t => t.DestinationStation)
                .WithMany(s => s.TripsTo)
                .HasForeignKey(t => t.DestinationStationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(t => t.Train)
                .WithMany(tr => tr.Trips)
                .HasForeignKey(t => t.TrainId);
        }
    }
}
