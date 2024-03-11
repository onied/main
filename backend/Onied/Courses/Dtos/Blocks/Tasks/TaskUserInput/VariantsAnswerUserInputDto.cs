namespace Courses.Dtos.Blocks.Tasks.TaskUserInput;

public class VariantsAnswerUserInputDto : UserInputDto
{
    public List<VariantDto> Variants { get; set; } = new();
}