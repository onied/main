using Courses.Models;

namespace Courses.Dtos.Blocks.Tasks.TaskUserInput;

public class UserInputDto
{
    // public int UserId { get; set; }
    public TaskType TaskType { get; set; }
    public int TaskId { get; set; }
    public bool IsDone { get; set; }
    
    public List<int>? VariantsIds { get; set; }
    public string? Answer { get; set; }
    public string? Text { get; set; }
}