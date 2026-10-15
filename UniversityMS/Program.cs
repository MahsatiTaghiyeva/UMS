using Microsoft.EntityFrameworkCore;
using UniversityManagementSystem.Data;

// ==================== MAIN MENU ====================

using (var context = new UniversityDbContext())
{
    DbInitializer.Initialize(context);
}
while (true)
{
    Console.WriteLine("\n========== UNIVERSITY MANAGEMENT SYSTEM ==========");
    Console.WriteLine("1.  Add Department");
    Console.WriteLine("2.  Add Teacher");
    Console.WriteLine("3.  Add Course");
    Console.WriteLine("4.  Add Student");
    Console.WriteLine("5.  Enroll Student in Course");
    Console.WriteLine("6.  Show Departments with Related Data");
    Console.WriteLine("7.  Show Students and Their Courses");
    Console.WriteLine("8.  Show Students by Course");
    Console.WriteLine("9.  Update Student");
    Console.WriteLine("10. Delete Student");
    Console.WriteLine("11. Test DeleteBehavior");
    Console.WriteLine("12. Show ChangeTracker and EntityState");
    Console.WriteLine("0.  Exit");
    Console.Write("Select an option: ");

    string? choice = Console.ReadLine();

    try
    {
        switch (choice)
        {
            case "1":
                AddDepartment();
                break;
            case "2":
                AddTeacher();
                break;
            case "3":
                AddCourse();
                break;
            case "4":
                AddStudent();
                break;
            case "5":
                EnrollStudentInCourse();
                break;
            case "6":
                ShowDepartmentsWithRelatedData();
                break;
            case "7":
                ShowStudentsAndTheirCourses();
                break;
            case "8":
                ShowStudentsByCourse();
                break;
            case "9":
                UpdateStudent();
                break;
            case "10":
                DeleteStudent();
                break;
            case "11":
                TestDeleteBehavior();
                break;
            case "12":
                ShowChangeTracker();
                break;
            case "0":
                Console.WriteLine("Goodbye!");
                return;
            default:
                Console.WriteLine("Invalid option. Try again.");
                break;
        }
    }
    catch (DbUpdateException ex)
    {
        Console.WriteLine(
            $"Database error: {ex.GetBaseException().Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}


void AddDepartment()
{
    using var context = new UniversityDbContext();

    Console.Write("Department name: ");
    string name = ReadRequiredText();

    Console.Write("Description: ");
    string description = ReadRequiredText();

    var department = new Department
    {
        Name = name,
        Description = description
    };

    context.Departments.Add(department);

    Console.WriteLine($"State after Add: {context.Entry(department).State}");

    context.SaveChanges();

    Console.WriteLine($"Department added. ID: {department.Id}");
    Console.WriteLine($"State after SaveChanges: {context.Entry(department).State}");
}


void AddTeacher()
{
    using var context = new UniversityDbContext();

    ShowDepartments(context);

    int departmentId = ReadInt("Department ID: ");

    if (!context.Departments.Any(d => d.Id == departmentId))
    {
        Console.WriteLine("Department not found.");
        return;
    }

    Console.Write("Teacher full name: ");
    string fullName = ReadRequiredText();

    Console.Write("Teacher email: ");
    string email = ReadRequiredText();

    var teacher = new Teacher
    {
        FullName = fullName,
        Email = email,
        DepartmentId = departmentId
    };

    context.Teachers.Add(teacher);
    context.SaveChanges();

    Console.WriteLine($"Teacher added.");
}


void AddCourse()
{
    using var context = new UniversityDbContext();

    ShowDepartments(context);

    int departmentId = ReadInt("Department ID: ");

    if (!context.Departments.Any(d => d.Id == departmentId))
    {
        Console.WriteLine("Department not found.");
        return;
    }

    var teachers = context.Teachers
        .Where(t => t.DepartmentId == departmentId)
        .ToList();

    if (teachers.Count == 0)
    {
        Console.WriteLine("Add a teacher to this department first.");
        return;
    }

    foreach (var teacher in teachers)
        Console.WriteLine($"{teacher.Id}. {teacher.FullName}");

    int teacherId = ReadInt("Teacher ID: ");

    if (!teachers.Any(t => t.Id == teacherId))
    {
        Console.WriteLine("Teacher not found in this department.");
        return;
    }

    Console.Write("Course name: ");
    string name = ReadRequiredText();

    int credit = ReadInt("Credits: ");

    if (credit <= 0)
    {
        Console.WriteLine("Credits must be greater than zero.");
        return;
    }

    var course = new Course
    {
        Name = name,
        Credit = credit,
        DepartmentId = departmentId,
        TeacherId = teacherId
    };

    context.Courses.Add(course);
    context.SaveChanges();

    Console.WriteLine($"Course added. ID: {course.Id}");
}

// ==================== 4. ADD STUDENT ====================

void AddStudent()
{
    using var context = new UniversityDbContext();

    Console.Write("Student full name: ");
    string fullName = ReadRequiredText();

    Console.Write("Student email: ");
    string email = ReadRequiredText();

    Console.Write("Birth date (yyyy-MM-dd): ");

    if (!DateTime.TryParse(Console.ReadLine(), out DateTime birthDate))
    {
        Console.WriteLine("Invalid date.");
        return;
    }

    var student = new Student
    {
        FullName = fullName,
        Email = email,
        BirthDate = birthDate
    };

    context.Students.Add(student);

    Console.WriteLine($"State after Add: {context.Entry(student).State}");

    context.SaveChanges();

    Console.WriteLine($"Student added. ID: {student.Id}");
    Console.WriteLine($"State after SaveChanges: {context.Entry(student).State}");
}


void EnrollStudentInCourse()
{
    using var context = new UniversityDbContext();

    ShowStudents(context);
    int studentId = ReadInt("Student ID: ");

    if (!context.Students.Any(s => s.Id == studentId))
    {
        Console.WriteLine("Student not found.");
        return;
    }

    ShowCourses(context);
    int courseId = ReadInt("Course ID: ");

    if (!context.Courses.Any(c => c.Id == courseId))
    {
        Console.WriteLine("Course not found.");
        return;
    }

    bool alreadyEnrolled = context.Enrollments.Any(e =>
        e.StudentId == studentId && e.CourseId == courseId);

    if (alreadyEnrolled)
    {
        Console.WriteLine("Student is already enrolled in this course.");
        return;
    }

    var enrollment = new Enrollment
    {
        StudentId = studentId,
        CourseId = courseId,
        EnrollmentDate = DateTime.UtcNow,
        Grade = 0
    };

    context.Enrollments.Add(enrollment);
    context.SaveChanges();

    Console.WriteLine("Student enrolled successfully.");
}


void ShowDepartmentsWithRelatedData()
{
    using var context = new UniversityDbContext();

    var departments = context.Departments
        .Include(d => d.Teachers)
        .Include(d => d.Courses)
            .ThenInclude(c => c.Teacher)
        .ToList();

    foreach (var department in departments)
    {
        Console.WriteLine($"\nDepartment: {department.Name}");
        Console.WriteLine($"Description: {department.Description}");

        Console.WriteLine("Teachers:");

        foreach (var teacher in department.Teachers)
            Console.WriteLine($"  {teacher.Id}. {teacher.FullName}");

        Console.WriteLine("Courses:");

        foreach (var course in department.Courses)
        {
            Console.WriteLine(
                $"  {course.Name} | Credits: {course.Credit} | " +
                $"Teacher: {course.Teacher.FullName}");
        }
    }
}


void ShowStudentsAndTheirCourses()
{
    using var context = new UniversityDbContext();

    var students = context.Students
        .Include(s => s.Enrollments)
            .ThenInclude(e => e.Course)
        .ToList();

    foreach (var student in students)
    {
        Console.WriteLine($"\nStudent: {student.FullName}");

        if (student.Enrollments.Count == 0)
        {
            Console.WriteLine("  No courses.");
            continue;
        }

        foreach (var enrollment in student.Enrollments)
        {
            Console.WriteLine(
                $"  Course: {enrollment.Course.Name} | " +
                $"Date: {enrollment.EnrollmentDate:yyyy-MM-dd} | " +
                $"Grade: {enrollment.Grade}");
        }
    }
}


void ShowStudentsByCourse()
{
    using var context = new UniversityDbContext();

    ShowCourses(context);
    int courseId = ReadInt("Course ID: ");

    var course = context.Courses
        .Include(c => c.Enrollments)
            .ThenInclude(e => e.Student)
        .FirstOrDefault(c => c.Id == courseId);

    if (course == null)
    {
        Console.WriteLine("Course not found.");
        return;
    }

    Console.WriteLine($"\nCourse: {course.Name}");

    foreach (var enrollment in course.Enrollments)
    {
        Console.WriteLine(
            $"{enrollment.Student.FullName} | " +
            $"{enrollment.Student.Email}");
    }
}


void UpdateStudent()
{
    using var context = new UniversityDbContext();

    ShowStudents(context);
    int studentId = ReadInt("Student ID: ");

    var student = context.Students
        .FirstOrDefault(s => s.Id == studentId);

    if (student == null)
    {
        Console.WriteLine("Student not found.");
        return;
    }

    Console.Write("New full name: ");
    student.FullName = ReadRequiredText();

    Console.Write("New email: ");
    student.Email = ReadRequiredText();

    Console.WriteLine(
        $"State before DetectChanges: {context.Entry(student).State}");

    context.ChangeTracker.DetectChanges();

    Console.WriteLine(
        $"State after DetectChanges: {context.Entry(student).State}");

    context.SaveChanges();

    Console.WriteLine("Student updated successfully.");
    Console.WriteLine($"State after SaveChanges: {context.Entry(student).State}");
}


void DeleteStudent()
{
    using var context = new UniversityDbContext();

    ShowStudents(context);
    int studentId = ReadInt("Student ID to delete: ");

    var student = context.Students
        .FirstOrDefault(s => s.Id == studentId);

    if (student == null)
    {
        Console.WriteLine("Student not found.");
        return;
    }

    Console.Write($"Delete {student.FullName}? (y/n): ");

    if (!string.Equals(Console.ReadLine()?.Trim(), "y",
            StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Deletion cancelled.");
        return;
    }

    context.Students.Remove(student);

    Console.WriteLine($"State after Remove: {context.Entry(student).State}");

    context.SaveChanges();

    Console.WriteLine("Student deleted successfully.");
    Console.WriteLine($"State after SaveChanges: {context.Entry(student).State}");
}


void TestDeleteBehavior()
{
    Console.WriteLine("\n1. Test Department Restrict");
    Console.WriteLine("2. Test Student Cascade");
    Console.Write("Select test: ");

    switch (Console.ReadLine())
    {
        case "1":
            TestDepartmentRestrict();
            break;
        case "2":
            TestStudentCascade();
            break;
        default:
            Console.WriteLine("Invalid option.");
            break;
    }
}

void TestDepartmentRestrict()
{
    using var context = new UniversityDbContext();

    ShowDepartments(context);
    int departmentId = ReadInt("Department ID: ");

    var department = context.Departments
        .FirstOrDefault(d => d.Id == departmentId);

    if (department == null)
    {
        Console.WriteLine("Department not found.");
        return;
    }

    Console.Write("Attempt deletion to test Restrict. Continue? (y/n): ");

    if (!string.Equals(Console.ReadLine()?.Trim(), "y",
            StringComparison.OrdinalIgnoreCase))
        return;

    context.Departments.Remove(department);

    try
    {
        context.SaveChanges();
        Console.WriteLine("Department deleted. No restricting related rows were present.");
    }
    catch (DbUpdateException ex)
    {
        Console.WriteLine("Deletion blocked by a database constraint.");
        Console.WriteLine(ex.GetBaseException().Message);
    }
}

void TestStudentCascade()
{
    using var context = new UniversityDbContext();

    ShowStudents(context);
    int studentId = ReadInt("Student ID to delete: ");

    var student = context.Students
        .Include(s => s.Enrollments)
        .FirstOrDefault(s => s.Id == studentId);

    if (student == null)
    {
        Console.WriteLine("Student not found.");
        return;
    }

    Console.WriteLine($"Student: {student.FullName}");
    Console.WriteLine($"Enrollments: {student.Enrollments.Count}");
    Console.Write("This permanently deletes the student. Continue? (y/n): ");

    if (!string.Equals(Console.ReadLine()?.Trim(), "y",
            StringComparison.OrdinalIgnoreCase))
        return;

    context.Students.Remove(student);
    context.SaveChanges();

    Console.WriteLine("Student deleted. Cascade should remove its enrollments.");
}


void ShowChangeTracker()
{
    using var context = new UniversityDbContext();

    var student = context.Students.FirstOrDefault();

    if (student == null)
    {
        Console.WriteLine("Add a student first.");
        return;
    }

    Console.WriteLine($"Initial state: {context.Entry(student).State}");

    var newStudent = new Student
    {
        FullName = "Tracker Test Student",
        Email = $"tracker{Guid.NewGuid():N}@example.com",
        BirthDate = new DateTime(2005, 1, 1)
    };

    context.Students.Add(newStudent);

    student.FullName += " (Test)";
    context.ChangeTracker.DetectChanges();

    Console.WriteLine("\nTracked entities:");

    foreach (var entry in context.ChangeTracker.Entries())
    {
        Console.WriteLine(
            $"Entity: {entry.Metadata.ClrType.Name} | State: {entry.State}");
    }

    Console.WriteLine("No SaveChanges was called; test changes are discarded.");
}

//helpers
void ShowDepartments(UniversityDbContext context)
{
    Console.WriteLine("\n--- DEPARTMENTS ---");

    foreach (var department in context.Departments.ToList())
        Console.WriteLine($"{department.Id}. {department.Name}");
}

void ShowStudents(UniversityDbContext context)
{
    Console.WriteLine("\n--- STUDENTS ---");

    foreach (var student in context.Students.ToList())
        Console.WriteLine($"{student.Id}. {student.FullName} | {student.Email}");
}

void ShowCourses(UniversityDbContext context)
{
    Console.WriteLine("\n--- COURSES ---");

    foreach (var course in context.Courses.ToList())
        Console.WriteLine($"{course.Id}. {course.Name} | Credits: {course.Credit}");
}

int ReadInt(string message)
{
    while (true)
    {
        Console.Write(message);

        if (int.TryParse(Console.ReadLine(), out int value))
            return value;

        Console.WriteLine("Please enter a valid number.");
    }
}

string ReadRequiredText()
{
    while (true)
    {
        string? input = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(input))
            return input.Trim();

        Console.Write("This field is required. Try again: ");
    }
}