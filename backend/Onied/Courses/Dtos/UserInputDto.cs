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
    
    public TaskType TaskType 
        => VariantsIds is not null 
            ? VariantsIds.Count == 1 
                ? TaskType.SingleAnswer 
                : TaskType.MultipleAnswers 
            : Answer is not null 
                ? TaskType.InputAnswer 
                : TaskType.ManualReview;
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (TaskType is TaskType.SingleAnswer && Answer is not null)
            yield return new ValidationResult(
                "Task with SingleAnswer shouldn't contain Answer or Text",
                new[] {nameof(Answer), nameof(Text) });
        
        if (TaskType is TaskType.SingleAnswer && Text is not null)
            yield return new ValidationResult(
                "Task with SingleAnswer shouldn't contain Text",
                new[] { nameof(Text) });
        
        if (TaskType is TaskType.MultipleAnswers && Answer is not null)
            yield return new ValidationResult(
                "Task with MultipleAnswers shouldn't contain Answer",
                new[] { nameof(Answer) });
        
        if (TaskType is TaskType.MultipleAnswers && Text is not null)
            yield return new ValidationResult(
                "Task with MultipleAnswers shouldn't contain Text",
                new[] { nameof(Text) });
        
        if (TaskType is TaskType.InputAnswer && VariantsIds is not null)
            yield return new ValidationResult(
                "Task with InputAnswer shouldn't contain VariantsIds",
                new[] { nameof(VariantsIds) });
        
        if (TaskType is TaskType.InputAnswer && Text is not null)
            yield return new ValidationResult(
                "Task with InputAnswer shouldn't contain Text",
                new[] { nameof(Text) });
        
        if (TaskType is TaskType.ManualReview && VariantsIds is not null)
            yield return new ValidationResult(
                "Task with ManualReview shouldn't contain VariantsIds",
                new[] { nameof(VariantsIds) });
        
        if (TaskType is TaskType.ManualReview && Answer is not null)
            yield return new ValidationResult(
                "Task with ManualReview shouldn't contain Answer",
                new[] { nameof(Answer) });
    }
}