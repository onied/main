namespace Courses.Models;

public class TaskVariant
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public VariantsTask Task { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsCorrect { get; set; }
}