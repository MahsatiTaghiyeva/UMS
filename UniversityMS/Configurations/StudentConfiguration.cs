using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace UniversityManagementSystem.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasColumnName("StudentId");

        builder.Property(s => s.FullName)
            .HasColumnName("FullName")
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(s => s.Email)
            .HasColumnName("Email")
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.BirthDate)
            .HasColumnName("BirthDate")
            .IsRequired();

        builder.HasIndex(s => s.Email)
            .IsUnique();
    }
}