using System.ComponentModel.DataAnnotations;

namespace Courses.Models.Blocks.Tasks.TaskUserInput;

public class ManualReviewUserInput : UserInput
{
    [MinLength(1)] 
    [MaxLength(15000)] 
    public string Text { get; set; } = null!;
}