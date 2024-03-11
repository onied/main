namespace Courses.Models.Blocks.Tasks.TaskUserInput;

public class VariantsAnswerUserInput : UserInput
{
    public ICollection<TaskVariant> Variants { get; set; } = new List<TaskVariant>();
    
    // TODO: прикрутить IValidateObject и проверку на Single/Multiple
}