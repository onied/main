namespace Courses.Models;

public class InputTask : Task
{
    public ICollection<TaskTextInputAnswer> Answers { get; } = new List<TaskTextInputAnswer>();
}