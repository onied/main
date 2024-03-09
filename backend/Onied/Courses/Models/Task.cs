namespace Courses.Models;

public class Task
{
    public int Id { get; set; }
    public int TasksBlockId { get; set; }
    public TasksBlock TasksBlock { get; set; } = null!;
    public TaskType TaskType { get; set; }
    public string Title { get; set; } = null!;
    public int? Points { get; set; } = null;
    public int MaxPoints { get; set; } = 1;
}