namespace Courses.Models;

public class Block
{
    public int Id { get; set; }
    public int ModuleId { get; set; }
    public Module Module { get; set; } = null!;
    public string Title { get; set; } = null!;
}