using Courses.Models;

namespace Courses.Dtos;

public class TaskDto
{
    public int Id { get; set; }
    public TaskType TaskType { get; set; }
    public string Title { get; set; } = null!;
    public int MaxPoints { get; set; }
    public List<VariantDto>? Variants { get; set; }
}