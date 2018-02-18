namespace P01_StudentSystem.Data.Configuration
{
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class HomeworkConfiguration : IEntityTypeConfiguration<Homework>
    {
        public void Configure(EntityTypeBuilder<Homework> builder)
        {
            builder
                .HasKey(h => h.HomeworkId);

            builder
                .Property(h => h.Content)
                .HasMaxLength(200)
                .IsUnicode(false)
                .IsRequired(true);

            builder
                .Property(h => h.ContentType)
                .IsRequired(true);

            builder
                .Property(h => h.SubmissionTime)
                .IsRequired(true);

            builder
                .HasOne(s => s.Student)
                .WithMany(h => h.HomeworkSubmissions);

            builder
                .HasOne(c => c.Course)
                .WithMany(h => h.HomeworkSubmissions);
        }
    }
}
