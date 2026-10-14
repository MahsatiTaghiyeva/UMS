using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments");

        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id)
            .HasColumnName("DepartmentId");

        builder.Property(d => d.Name)
            .HasColumnName("DepartmentName")
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(d => d.Name)
        .IsUnique();

        builder.Property(d => d.Description)
            .HasColumnName("Description")
            .IsRequired()
            .HasMaxLength(500);

        builder.HasMany(d => d.Teachers)
        .WithOne(t => t.Department)
        .HasForeignKey(t => t.DepartmentId)
        .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.Courses)
            .WithOne(c => c.Department)
            .HasForeignKey(c => c.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}