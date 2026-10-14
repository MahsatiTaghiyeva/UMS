using Microsoft.EntityFrameworkCore;
using UniversityManagementSystem.Data;
namespace UniversityManagementSystem.Services;

public class DeleteBehaviorService
{
    private readonly UniversityDbContext _context;

    public DeleteBehaviorService(UniversityDbContext context)
    {
        _context = context;
    }

    // Scenario 1: Delete a Department that has Teachers and Courses
    public async Task TestDepartmentDeleteAsync(int departmentId)
    {
        Console.WriteLine("\n--- Department Delete Test ---");

        var department = await _context.Departments
            .Include(d => d.Teachers)
            .Include(d => d.Courses)
            .FirstOrDefaultAsync(d => d.Id == departmentId);

        if (department == null)
        {
            Console.WriteLine("Department not found.");
            return;
        }

        try
        {
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            Console.WriteLine("Department deleted successfully.");
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine(
                "Department could not be deleted because related records exist.");

            Console.WriteLine($"Reason: {ex.GetBaseException().Message}");

            _context.ChangeTracker.Clear();
        }
    }

    // Scenario 2: Delete a Course that has Enrollments
    public async Task TestCourseDeleteAsync(int courseId)
    {
        Console.WriteLine("\n--- Course Delete Test ---");

        var course = await _context.Courses
            .Include(c => c.Enrollments)
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
        {
            Console.WriteLine("Course not found.");
            return;
        }

        var enrollmentCount = course.Enrollments.Count;

        try
        {
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            Console.WriteLine("Course deleted successfully.");

            Console.WriteLine(
                $"{enrollmentCount} related Enrollment record(s) were deleted.");
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine("Course deletion failed.");
            Console.WriteLine($"Reason: {ex.GetBaseException().Message}");

            _context.ChangeTracker.Clear();
        }
    }

    // Scenario 3: Delete a Student that has Enrollments
    public async Task TestStudentDeleteAsync(int studentId)
    {
        Console.WriteLine("\n--- Student Delete Test ---");

        var student = await _context.Students
            .Include(s => s.Enrollments)
            .FirstOrDefaultAsync(s => s.Id == studentId);

        if (student == null)
        {
            Console.WriteLine("Student not found.");
            return;
        }

        var enrollmentCount = student.Enrollments.Count;

        try
        {
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            Console.WriteLine("Student deleted successfully.");

            Console.WriteLine(
                $"{enrollmentCount} related Enrollment record(s) were deleted.");
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine("Student deletion failed.");
            Console.WriteLine($"Reason: {ex.GetBaseException().Message}");

            _context.ChangeTracker.Clear();
        }
    }
}