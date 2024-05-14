namespace Courses.Dtos.Course.Response;

public class ModuleResponse
{
    public int Id { get; set; }
    public int Index { get; set; }
    public string Title { get; set; } = null!;
    public List<BlockResponse> Blocks { get; init; } = new();
}
