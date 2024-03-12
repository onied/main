namespace Courses.Dtos.Blocks.Tasks.TaskUserInput;

public class VariantsAnswerUserInputDto : UserInputDto
{
    public new List<int> VariantsIds { get; set; } = new();
}