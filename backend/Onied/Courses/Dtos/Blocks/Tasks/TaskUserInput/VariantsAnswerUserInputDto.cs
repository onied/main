namespace Courses.Dtos.Blocks.Tasks.TaskUserInput;

public class VariantsAnswerUserInputDto : UserInputDto
{
    public new List<VariantDto> Variants { get; set; } = new();
}