using System.ComponentModel.DataAnnotations;

namespace Courses.Models;

public class Task
{
    public int Id { get; set; }

    public int TasksBlockId { get; set; }
    public TasksBlock TasksBlock { get; set; } = null!;

    public TaskType TaskType { get; set; }

    [MinLength(1)]
    [MaxLength(280)]
    public string Title { get; set; } = null!;

    [Range(0, 1000)]
    public int MaxPoints { get; set; } = 1;
}
