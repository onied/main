namespace Courses.Dtos.TaskCheckDtos;

public class BlockDto
{
    public ModuleDto Module { get; set; } = null!;
    public int Index { get; set; }
    public string Title { get; set; } = null!;
}
