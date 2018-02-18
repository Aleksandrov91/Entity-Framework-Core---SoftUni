namespace P03_FootballBetting.Data.Configuration
{
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class PlayerStatisticConfiguration : IEntityTypeConfiguration<PlayerStatistic>
    {
        public void Configure(EntityTypeBuilder<PlayerStatistic> builder)
        {
            builder
                .HasKey(pc => new { pc.GameId, pc.PlayerId });

            builder
                .HasOne(pc => pc.Game)
                .WithMany(g => g.PlayerStatistics)
                .HasForeignKey(g => g.GameId);

            builder
                .HasOne(pc => pc.Player)
                .WithMany(p => p.PlayerStatistics)
                .HasForeignKey(p => p.PlayerId);
        }
    }
}
