namespace P03_FootballBetting.Data.Configuration
{
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder
                .HasKey(t => t.TeamId);

            builder
                .Property(t => t.Name)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(50);

            builder
                .Property(t => t.Initials)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(5);

            builder
                .Property(t => t.LogoUrl)
                .IsUnicode(false)
                .IsRequired(false)
                .HasMaxLength(100);

            builder
                .HasOne(t => t.PrimaryKitColor)
                .WithMany(c => c.PrimaryKitTeams);

            builder
                .HasOne(t => t.SecondaryKitColor)
                .WithMany(c => c.SecondaryKitTeams)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(t => t.Town)
                .WithMany(t => t.Teams);
        }
    }
}
