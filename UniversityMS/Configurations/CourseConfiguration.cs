using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace UniversityManagementSystem.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Courses");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("CourseId");
        
        builder.Property(c => c.Name)
            .HasColumnName("CourseName")
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(c => c.Credit)
            .HasColumnName("Credit")
            .HasPrecision(3, 1)
            .IsRequired();

        builder.Property(c => c.DepartmentId)
            .HasColumnName("DepartmentId")
            .IsRequired();

        builder.Property(c => c.TeacherId)
            .HasColumnName("TeacherId")
            .IsRequired();
    }
}