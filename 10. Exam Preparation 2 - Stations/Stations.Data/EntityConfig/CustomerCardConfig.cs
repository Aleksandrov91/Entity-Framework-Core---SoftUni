namespace Stations.Data.EntityConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;
    using Models.Enums;

    internal class CustomerCardConfig : IEntityTypeConfiguration<CustomerCard>
    {
        public void Configure(EntityTypeBuilder<CustomerCard> builder)
        {
            builder.HasKey(cc => cc.Id);

            builder.Property(cc => cc.Name)
                .HasMaxLength(128)
                .IsRequired()
                .IsUnicode();

            builder.Property(cc => cc.Type)
                .HasDefaultValue(CardType.Normal);
        }
    }
}
