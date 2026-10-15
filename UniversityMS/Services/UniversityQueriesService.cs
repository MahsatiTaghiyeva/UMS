using Microsoft.EntityFrameworkCore;
using UniversityManagementSystem.Data;

namespace UniversityManagementSystem.Services;

public class UniversityQueryService
{
    private readonly UniversityDbContext _context;

    public UniversityQueryService(UniversityDbContext context)
    {
        _context = context;
    }

    public async Task ShowDepartmentsWithTeachersAsync()
    {
        var departments = await _context.Departments
            .Include(d => d.Teachers)
            .ToListAsync();

        foreach (var department in departments)
        {
            Console.WriteLine($"\nDepartment: {department.Name}");
            Console.WriteLine("Teachers:");

            foreach (var teacher in department.Teachers)
            {
                Console.WriteLine(
                    $"  {teacher.FullName} - {teacher.Email}");
            }
        }
    }

    public async Task ShowDepartmentsWithCoursesAndTeachersAsync()
    {
        var departments = await _context.Departments
            .Include(d => d.Courses)
                .ThenInclude(c => c.Teacher)
            .ToListAsync();

        foreach (var department in departments)
        {
            Console.WriteLine($"\nDepartment: {department.Name}");

            foreach (var course in department.Courses)
            {
                Console.WriteLine($"  Course: {course.Name}");
                Console.WriteLine($"  Credit: {course.Credit}");
                Console.WriteLine(
                    $"  Teacher: {course.Teacher.FullName}");
            }
        }
    }

    public async Task ShowStudentsWithEnrollmentsAsync()
    {
        var students = await _context.Students
            .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
            .ToListAsync();

        foreach (var student in students)
        {
            Console.WriteLine($"\nStudent: {student.FullName}");

            foreach (var enrollment in student.Enrollments)
            {
                Console.WriteLine(
                    $"  Course: {enrollment.Course.Name}");

                Console.WriteLine(
                    $"  Enrollment Date: " +
                    $"{enrollment.EnrollmentDate:yyyy-MM-dd}");

                Console.WriteLine($"  Grade: {enrollment.Grade}");
            }
        }
    }

    public async Task ShowCoursesWithStudentsAsync()
    {
        var courses = await _context.Courses
            .Include(c => c.Enrollments)
                .ThenInclude(e => e.Student)
            .ToListAsync();

        foreach (var course in courses)
        {
            Console.WriteLine($"\nCourse: {course.Name}");
            Console.WriteLine("Students:");

            foreach (var enrollment in course.Enrollments)
            {
                Console.WriteLine(
                    $"  {enrollment.Student.FullName}");
            }
        }
    }

    public async Task ShowStudentCourseDetailsAsync()
    {
        var students = await _context.Students
            .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
            .ToListAsync();

        foreach (var student in students)
        {
            Console.WriteLine($"\nStudent: {student.FullName}");

            foreach (var enrollment in student.Enrollments)
            {
                Console.WriteLine(
                    $"  Course: {enrollment.Course.Name}");

                Console.WriteLine(
                    $"  Enrollment Date: " +
                    $"{enrollment.EnrollmentDate:yyyy-MM-dd}");

                Console.WriteLine($"  Grade: {enrollment.Grade}");
            }
        }
    }
}