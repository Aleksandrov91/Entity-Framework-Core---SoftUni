namespace Employees.Data.EntityConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Model;

    internal class EmployeeConfig : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.EmployeeId);

            builder.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsRequired()
                .IsUnicode();

            builder.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsRequired()
                .IsUnicode();

            builder.Property(e => e.Salary)
                .IsRequired();

            builder.Property(e => e.Birthday)
                .HasColumnType("DATE")
                .IsRequired(false);

            builder.Property(e => e.Address)
                .HasMaxLength(100)
                .IsRequired(false)
                .IsUnicode();
        }
    }
}
