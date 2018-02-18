namespace Stations.Data.EntityConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    internal class TrainSeatConfig : IEntityTypeConfiguration<TrainSeat>
    {
        public void Configure(EntityTypeBuilder<TrainSeat> builder)
        {
            builder.HasKey(ts => ts.Id);

            builder.Property(t => t.TrainId)
                .IsRequired();

            builder.Property(t => t.SeatingClassId)
                .IsRequired();

            builder.Property(t => t.Quantity)
                .IsRequired();

            builder
                .HasOne(ts => ts.Train)
                .WithMany(t => t.TrainSeats)
                .HasForeignKey(ts => ts.TrainId);
        }
    }
}
