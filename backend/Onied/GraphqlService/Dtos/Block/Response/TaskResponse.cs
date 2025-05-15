using Courses.Data.Models;

namespace GraphqlService.Dtos.Block.Response;

public class TaskResponse
{
    public int Id { get; set; }
    public TaskType TaskType { get; set; }
    public string Title { get; set; } = null!;
    public int Points { get; set; }
    public int MaxPoints { get; set; }
    public List<VariantResponse>? Variants { get; set; }
}
