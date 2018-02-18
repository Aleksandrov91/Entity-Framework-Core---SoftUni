namespace P01_StudentSystem.Data.Configuration
{
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder
                .HasKey(s => s.StudentId);

            builder
                .Property(s => s.Name)
                .HasMaxLength(100)
                .IsUnicode(true)
                .IsRequired(true);

            builder
                .Property(s => s.PhoneNumber)
                .HasColumnType("CHAR(10)")
                .IsUnicode(false)
                .IsRequired(false);

            builder
                .Property(s => s.RegisteredOn)
                .HasDefaultValueSql("GETDATE()");

            builder
                .Property(s => s.Birthday)
                .IsRequired(false);

            builder
                .HasMany(c => c.CourseEnrollments)
                .WithOne(s => s.Student);

            builder
                .HasMany(h => h.HomeworkSubmissions)
                .WithOne(s => s.Student);
        }
    }
}
