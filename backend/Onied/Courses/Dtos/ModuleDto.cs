namespace Courses.Dtos;

public class ModuleDto
{
    public string Title { get; set; } = null!;
    public List<BlockDto> Blocks { get; } = new();
}
