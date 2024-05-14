using Courses.Models;

namespace Courses.Dtos.EditCourse.Request;

public class EditTaskRequest
{
    public int Id { get; set; }
    public TaskType TaskType { get; set; }
    public string Title { get; set; } = null!;
    public bool IsNew { get; set; }
    public int MaxPoints { get; set; }
    public bool? IsNumber { get; set; }
    public int? Accuracy { get; set; }
    public bool? IsCaseSensitive { get; set; }
    public List<EditAnswersRequest>? Variants { get; set; } = new();
    public List<EditAnswersRequest>? Answers { get; set; } = new();
}
