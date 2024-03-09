namespace Courses.Models;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public ICollection<Module> Modules { get; } = new List<Module>();
}