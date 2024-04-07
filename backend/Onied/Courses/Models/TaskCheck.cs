namespace Courses.Models;

public class TaskCheck
{
    public Guid Id { get; set; }
    public User Student { get; set; } = null!;
    public Task Task { get; set; } = null!;
    public string Contents { get; set; } = null!;
    public bool Checked { get; set; }
    public int Points { get; set; }
}
