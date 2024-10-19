using System.ComponentModel.DataAnnotations;

namespace Courses.Data.Models;

public class TaskTextInputAnswer
{
    public int Id { get; set; }

    public int TaskId { get; set; }
    public InputTask Task { get; set; } = null!;

    [MinLength(1)]
    [MaxLength(200)]
    public string Answer { get; set; } = null!;
}
