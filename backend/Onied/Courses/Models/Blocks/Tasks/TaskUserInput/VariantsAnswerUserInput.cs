namespace Courses.Models.Blocks.Tasks.TaskUserInput;

public class VariantsAnswerUserInput : UserInput
{
    public ICollection<TaskVariant> Variants { get; set; } = new List<TaskVariant>();
}