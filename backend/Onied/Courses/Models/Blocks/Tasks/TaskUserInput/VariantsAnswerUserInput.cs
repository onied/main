namespace Courses.Models.Blocks.Tasks.TaskUserInput;

public class VariantsAnswerUserInput : UserInput
{
    public ICollection<int> VariantsIds { get; set; } = new List<int>();
    
    // TODO: прикрутить IValidateObject и проверку на Single/Multiple
}