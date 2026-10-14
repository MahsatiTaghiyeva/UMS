public class Department
{
    public int Id {get; set;}
    public string Name {get; set;} = null!;
    public string Description {get; set;} = null!;
    public List<Teacher> Teachers =new List<Teacher>();
    public List<Course> Courses =new List<Course>();
}