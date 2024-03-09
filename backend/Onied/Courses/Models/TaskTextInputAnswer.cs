namespace Courses.Models;

public class TaskTextInputAnswer
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public InputTask Task { get; set; } = null!;
    public string Answer { get; set; } = null!;
    public bool IsCaseSensitive { get; set; } = false;
}