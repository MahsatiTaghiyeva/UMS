using Microsoft.EntityFrameworkCore;

namespace UniversityManagementSystem.Data;

public class UniversityDbContext : DbContext
{
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(
                "Host=localhost;" +
                "Port=5432;" +
                "Database=UniversityManagementDB;" +
                "Username=eventuser;" +
                "Password=moonlight591_Studio");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply existing Fluent API configurations.
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(UniversityDbContext).Assembly);

        // BirthDate should be stored as a PostgreSQL DATE.
        modelBuilder.Entity<Student>()
            .Property(s => s.BirthDate)
            .HasColumnType("date");
    }
}