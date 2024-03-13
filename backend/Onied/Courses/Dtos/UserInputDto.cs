using System.ComponentModel.DataAnnotations;
using Courses.Models;

namespace Courses.Dtos;

public class UserInputDto : IValidatableObject
{
    public int UserId { get; set; } = -1;
    
    public int TaskId { get; set; }
    public bool IsDone { get; set; }
    
    public List<int>? VariantsIds { get; set; }
    public string? Answer { get; set; }
    public string? Text { get; set; }
    
    public TaskType TaskType { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (TaskType is TaskType.SingleAnswer 
            && !(Text is not null && Answer is not null))
            yield return new ValidationResult(
                "Task with SingleAnswer should contain only VariantIds");
        
        if (TaskType is TaskType.MultipleAnswers 
            && !(Text is not null && Answer is not null))
            yield return new ValidationResult(
                "Task with MultipleAnswers should only VariantIds");
        
        if (TaskType is TaskType.InputAnswer
            && !(VariantsIds is not null && Text is not null))
            yield return new ValidationResult(
                "Task with InputAnswer should contain only Answer");
        
        if (TaskType is TaskType.ManualReview 
            && !(VariantsIds is not null && Answer is not null))
            yield return new ValidationResult(
                "Task with ManualReview should contain only Text");
    }
}