namespace Courses.Models;

public class VariantsTask : Task
{
    public ICollection<TaskVariant> Variants { get; } = new List<TaskVariant>();
}
