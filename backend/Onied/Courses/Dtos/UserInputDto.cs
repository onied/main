using System.ComponentModel.DataAnnotations;
using Courses.Models;

namespace Courses.Dtos;

public class UserInputDto : IValidatableObject
{
    public Guid UserId { get; set; } = Guid.NewGuid();

    public int TaskId { get; set; }
    public bool IsDone { get; set; }

    public List<int>? VariantsIds { get; set; }
    public string? Answer { get; set; }
    public string? Text { get; set; }

    public TaskType TaskType { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (TaskType is TaskType.SingleAnswer
            && !(Text is null && Answer is null))
            yield return new ValidationResult(
                "Task with SingleAnswer should contain only VariantIds");

        if (TaskType is TaskType.MultipleAnswers
            && !(Text is null && Answer is null))
            yield return new ValidationResult(
                "Task with MultipleAnswers should contain only VariantIds");

        if (TaskType is TaskType.InputAnswer
            && !(VariantsIds is null && Text is null))
            yield return new ValidationResult(
                "Task with InputAnswer should contain only Answer");

        if (TaskType is TaskType.ManualReview
            && !(VariantsIds is null && Answer is null))
            yield return new ValidationResult(
                "Task with ManualReview should contain only Text");
    }
}
