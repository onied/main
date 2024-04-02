namespace Courses.Dtos;

public class ModuleDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public List<BlockDto> Blocks { get; } = new();
}
