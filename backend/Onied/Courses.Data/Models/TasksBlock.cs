namespace Courses.Data.Models;

public class TasksBlock : Block
{
    public ICollection<Task> Tasks { get; } = new List<Task>();
}
