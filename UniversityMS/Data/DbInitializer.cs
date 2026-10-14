using Microsoft.EntityFrameworkCore;
namespace UniversityManagementSystem.Data;

public static class DbInitializer
{
    public static void Initialize(UniversityDbContext context)
    {
        // Apply pending migrations.
        context.Database.Migrate();

        Console.WriteLine("\nInitializing database...\n");

        // ==========================================
        // 1. DEPARTMENTS
        // ==========================================

        var computerScience = context.Departments
            .FirstOrDefault(d => d.Name == "Computer Science");

        if (computerScience == null)
        {
            computerScience = new Department
            {
                Name = "Computer Science",
                Description = "Computer Science Department"
            };

            context.Departments.Add(computerScience);
            context.SaveChanges();

            Console.WriteLine("Computer Science department added.");
        }

        var mathematics = context.Departments
            .FirstOrDefault(d => d.Name == "Mathematics");

        if (mathematics == null)
        {
            mathematics = new Department
            {
                Name = "Mathematics",
                Description = "Mathematics Department"
            };

            context.Departments.Add(mathematics);
            context.SaveChanges();

            Console.WriteLine("Mathematics department added.");
        }

        // ==========================================
        // 2. TEACHERS
        // ==========================================

        var teacher1 = context.Teachers
            .FirstOrDefault(t =>
                t.Email == "ali.mammadov@university.com");

        if (teacher1 == null)
        {
            teacher1 = new Teacher
            {
                FullName = "Ali Mammadov",
                Email = "ali.mammadov@university.com",
                DepartmentId = computerScience.Id
            };

            context.Teachers.Add(teacher1);
            context.SaveChanges();

            Console.WriteLine("Ali Mammadov added.");
        }

        var teacher2 = context.Teachers
            .FirstOrDefault(t =>
                t.Email == "leyla.aliyeva@university.com");

        if (teacher2 == null)
        {
            teacher2 = new Teacher
            {
                FullName = "Leyla Aliyeva",
                Email = "leyla.aliyeva@university.com",
                DepartmentId = mathematics.Id
            };

            context.Teachers.Add(teacher2);
            context.SaveChanges();

            Console.WriteLine("Leyla Aliyeva added.");
        }

        // ==========================================
        // 3. COURSES
        // ==========================================

        var course1 = context.Courses
            .FirstOrDefault(c => c.Name == "Programming");

        if (course1 == null)
        {
            course1 = new Course
            {
                Name = "Programming",
                Credit = 6,
                DepartmentId = computerScience.Id,
                TeacherId = teacher1.Id
            };

            context.Courses.Add(course1);
            context.SaveChanges();

            Console.WriteLine("Programming course added.");
        }

        var course2 = context.Courses
            .FirstOrDefault(c => c.Name == "Calculus");

        if (course2 == null)
        {
            course2 = new Course
            {
                Name = "Calculus",
                Credit = 5,
                DepartmentId = mathematics.Id,
                TeacherId = teacher2.Id
            };

            context.Courses.Add(course2);
            context.SaveChanges();

            Console.WriteLine("Calculus course added.");
        }

        // ==========================================
        // 4. STUDENTS
        // ==========================================

        var student1 = context.Students
            .FirstOrDefault(s =>
                s.Email == "aysel.hasanli@student.com");

        if (student1 == null)
        {
            student1 = new Student
            {
                FullName = "Aysel Hasanli",
                Email = "aysel.hasanli@student.com",
                BirthDate = new DateTime(2005, 4, 15)
            };

            context.Students.Add(student1);
            context.SaveChanges();

            Console.WriteLine("Aysel Hasanli added.");
        }

        var student2 = context.Students
            .FirstOrDefault(s =>
                s.Email == "murad.karimov@student.com");

        if (student2 == null)
        {
            student2 = new Student
            {
                FullName = "Murad Karimov",
                Email = "murad.karimov@student.com",
                BirthDate = new DateTime(2004, 8, 20)
            };

            context.Students.Add(student2);
            context.SaveChanges();

            Console.WriteLine("Murad Karimov added.");
        }

        // ==========================================
        // 5. ENROLLMENTS
        // ==========================================

        bool enrollment1Exists = context.Enrollments.Any(e =>
            e.StudentId == student1.Id &&
            e.CourseId == course1.Id);

        if (!enrollment1Exists)
        {
            var enrollment1 = new Enrollment
            {
                StudentId = student1.Id,
                CourseId = course1.Id,

                // PostgreSQL timestamp with time zone requires UTC.
                EnrollmentDate = DateTime.UtcNow,

                Grade = 90
            };

            context.Enrollments.Add(enrollment1);
            context.SaveChanges();

            Console.WriteLine(
                "Aysel Hasanli enrolled in Programming.");
        }

        bool enrollment2Exists = context.Enrollments.Any(e =>
            e.StudentId == student2.Id &&
            e.CourseId == course2.Id);

        if (!enrollment2Exists)
        {
            var enrollment2 = new Enrollment
            {
                StudentId = student2.Id,
                CourseId = course2.Id,

                // PostgreSQL timestamp with time zone requires UTC.
                EnrollmentDate = DateTime.UtcNow,

                Grade = 85
            };

            context.Enrollments.Add(enrollment2);
            context.SaveChanges();

            Console.WriteLine(
                "Murad Karimov enrolled in Calculus.");
        }

        // ==========================================
        // 6. COMPLETED
        // ==========================================

        Console.WriteLine(
            "\nDatabase initialization completed successfully!");
    }
}