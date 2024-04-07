using System.ComponentModel.DataAnnotations;
using Courses.Models;

namespace Courses.Dtos;

public class EditTaskDto
{
    public int Id { get; set; }
    public TaskType TaskType { get; set; }
    public string Title { get; set; } = null!;
    public bool IsNew { get; set; }
    public int MaxPoints { get; set; }
    public bool? IsNumber { get; set; }
    public int? Accuracy { get; set; }
    public bool? IsCaseSensitive { get; set; }
    public List<EditAnswersDto>? Variants { get; set; } = new();
    public List<EditAnswersDto>? Answers { get; set; } = new();
}
