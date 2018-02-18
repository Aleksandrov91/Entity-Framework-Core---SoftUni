namespace P01_StudentSystem.Data.Configuration
{
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder
                .HasKey(c => c.CourseId);

            builder
                .Property(c => c.Name)
                .HasMaxLength(80)
                .IsUnicode(true)
                .IsRequired(true);

            builder
                .Property(c => c.Description)
                .HasMaxLength(200)
                .IsRequired(false)
                .IsUnicode(true);

            builder
                .Property(c => c.StartDate)
                .IsRequired(true);

            builder
                .Property(c => c.EndDate)
                .IsRequired(true);

            builder
                .Property(c => c.Price)
                .IsRequired(true);

            builder
                .HasMany(s => s.StudentsEnrolled)
                .WithOne(c => c.Course);

            builder
                .HasMany(r => r.Resources)
                .WithOne(c => c.Course);

            builder
                .HasMany(h => h.HomeworkSubmissions)
                .WithOne(c => c.Course);

            builder
                .HasMany(s => s.StudentsEnrolled)
                .WithOne(c => c.Course);
        }
    }
}
