namespace Stations.Data.EntityConfig
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    internal class TrainConfig : IEntityTypeConfiguration<Train>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Train> builder)
        {
            builder.HasKey(t => t.Id);

            builder.HasIndex(t => t.TrainNumber).IsUnique();

            builder.Property(t => t.TrainNumber)
                .HasMaxLength(10)
                .IsRequired()
                .IsUnicode();

            builder.Property(t => t.Type)
                .IsRequired(false);
        }
    }
}
