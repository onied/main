namespace Courses.Models;

public class TaskTextInputAnswer
{
    public int Id { get; set; }
    public InputTask InputTask { get; set; } = null!;
    public string Answer { get; set; } = null!;
    public bool IsCaseSensitive { get; set; } = false;
}