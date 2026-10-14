public class Teacher
{
    public int Id {get; set;}
    public string FullName {get; set;} = null!;
    public string Email {get; set;} = null!;
    public int DepartmentId {get; set;}
    public Department Department {get; set;} = null!;
    public List<Course> Courses =new List<Course>();
}