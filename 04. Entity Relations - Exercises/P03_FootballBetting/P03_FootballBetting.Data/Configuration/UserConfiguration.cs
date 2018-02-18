namespace P03_FootballBetting.Data.Configuration
{
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(u => u.UserId);

            builder
                .Property(u => u.Username)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(30);

            builder
                .Property(u => u.Password)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(50);

            builder
                .HasAlternateKey(u => u.Email);

            builder
                .Property(u => u.Email)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(50);

            builder
                .Property(u => u.Name)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(50);
        }
    }
}
