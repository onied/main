namespace Courses.Data.Models;

public class VariantsTask : Task
{
    public ICollection<TaskVariant> Variants { get; } = new List<TaskVariant>();
}
