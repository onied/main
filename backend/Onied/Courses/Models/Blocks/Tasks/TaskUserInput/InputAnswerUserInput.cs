using System.ComponentModel.DataAnnotations;

namespace Courses.Models.Blocks.Tasks.TaskUserInput;

public class InputAnswerUserInput : UserInput
{
    [MinLength(1)]
    [MaxLength(200)]
    public string Answer { get; set; } = null!;
}