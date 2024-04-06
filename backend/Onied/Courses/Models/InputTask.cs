using System.ComponentModel.DataAnnotations;

namespace Courses.Models;

public class InputTask : Task
{
    public ICollection<TaskTextInputAnswer> Answers { get; } = new List<TaskTextInputAnswer>();
    public bool IsNumber { get; set; }
    [Range(0, 1000)]
    public int? Accuracy { get; set; }
    public bool IsCaseSensitive { get; set; }
}
