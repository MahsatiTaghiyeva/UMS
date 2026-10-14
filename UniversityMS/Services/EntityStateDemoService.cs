using Microsoft.EntityFrameworkCore;
using UniversityManagementSystem.Data;
namespace UniversityManagementSystem.Services;

public class EntityStateDemoService
{
    public void Run()
    {
        using var context = new UniversityDbContext();

        // A. ADDED
        Console.WriteLine("\n--- A. ADDED ---");

        var newStudent = new Student
        {
            FullName = "Test Student",
            Email = $"test{Guid.NewGuid():N}@example.com",
            BirthDate = new DateTime(2005, 5, 15)
        };

        Console.WriteLine(context.Entry(newStudent).State);
        // Detached

        context.Students.Add(newStudent);

        Console.WriteLine(context.Entry(newStudent).State);
        // Added

        context.SaveChanges();

        Console.WriteLine(context.Entry(newStudent).State);
        // Unchanged


        // B. MODIFIED
        Console.WriteLine("\n--- B. MODIFIED ---");

        var studentToUpdate = context.Students
            .FirstOrDefault(s => s.Email == newStudent.Email);

        if (studentToUpdate is not null)
        {
            Console.WriteLine(context.Entry(studentToUpdate).State);
            // Unchanged

            studentToUpdate.FullName = "Updated Test Student";

            Console.WriteLine(context.Entry(studentToUpdate).State);
            // EF Core adətən bu mərhələdə də Unchanged göstərir.

            context.ChangeTracker.DetectChanges();

            Console.WriteLine(context.Entry(studentToUpdate).State);
            // Modified

            context.SaveChanges();

            Console.WriteLine(context.Entry(studentToUpdate).State);
            // Unchanged
        }


        // C. DELETED
        Console.WriteLine("\n--- C. DELETED ---");

        var studentToDelete = context.Students
            .FirstOrDefault(s => s.Email == newStudent.Email);

        if (studentToDelete is not null)
        {
            context.Students.Remove(studentToDelete);

            Console.WriteLine(context.Entry(studentToDelete).State);
            // Deleted

            context.SaveChanges();

            Console.WriteLine(context.Entry(studentToDelete).State);
            // Detached
        }


        // D. UNCHANGED
        Console.WriteLine("\n--- D. UNCHANGED ---");

        var existingStudent = context.Students.FirstOrDefault();

        if (existingStudent is not null)
        {
            Console.WriteLine(context.Entry(existingStudent).State);
            // Unchanged
        }


        // E. DETACHED
        Console.WriteLine("\n--- E. DETACHED ---");

        var detachedStudent = new Student
        {
            FullName = "Detached Student",
            Email = $"detached{Guid.NewGuid():N}@example.com",
            BirthDate = new DateTime(2004, 3, 10)
        };

        Console.WriteLine(context.Entry(detachedStudent).State);
        // Detached

        context.Students.Attach(detachedStudent);

        Console.WriteLine(context.Entry(detachedStudent).State);
        // Unchanged


        // F. CHANGETRACKER
        Console.WriteLine("\n--- F. CHANGETRACKER ---");

        foreach (var entry in context.ChangeTracker.Entries())
        {
            Console.WriteLine(
                $"Entity: {entry.Metadata.ClrType.Name}, State: {entry.State}");
        }
    }
}