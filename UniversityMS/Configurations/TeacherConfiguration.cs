using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace UniversityManagementSystem.Configurations;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.ToTable("Teachers");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("TeacherId");

        builder.Property(t => t.FullName)
            .HasColumnName("FullName")
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(t => t.Email)
            .HasColumnName("Email")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.DepartmentId)
            .HasColumnName("DepartmentId")
            .IsRequired();

        builder.HasIndex(t => t.Email)
            .IsUnique();

        builder.HasMany(t => t.Courses)
            .WithOne(c => c.Teacher)
            .HasForeignKey(c => c.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}