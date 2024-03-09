namespace Courses.Models;

public class Module
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
    public ICollection<Block> Blocks { get; } = new List<Block>();
    public string Title { get; set; } = null!;
}