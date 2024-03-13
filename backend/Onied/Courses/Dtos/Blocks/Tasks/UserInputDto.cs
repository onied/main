using Courses.Models;

namespace Courses.Dtos.Blocks.Tasks;

public class UserInputDto
{
    public int UserId { get; set; } = -1;
    
    public int TaskId { get; set; }
    public bool IsDone { get; set; }
    
    public List<int>? VariantsIds { get; set; }
    public string? Answer { get; set; }
    public string? Text { get; set; }
    
    public TaskType TaskType 
        => VariantsIds is not null 
            ? VariantsIds.Count == 1 
                ? TaskType.SingleAnswer 
                : TaskType.MultipleAnswers 
            : Answer is not null 
                ? TaskType.ManualReview
                : TaskType.InputAnswer;
}