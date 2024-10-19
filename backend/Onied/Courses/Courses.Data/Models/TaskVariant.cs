using System.ComponentModel.DataAnnotations;

namespace Courses.Data.Models;

public class TaskVariant
{
    public int Id { get; set; }

    public int TaskId { get; set; }
    public VariantsTask Task { get; set; } = null!;

    [MinLength(1)]
    [MaxLength(280)]
    public string Description { get; set; } = null!;

    public bool IsCorrect { get; set; }
}
