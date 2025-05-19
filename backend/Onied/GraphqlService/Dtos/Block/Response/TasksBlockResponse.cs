using Courses.Data.Models;

namespace GraphqlService.Dtos.Block.Response;

public class TasksBlockResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public BlockType BlockType { get; set; }
    public bool IsCompleted { get; set; }
    public List<TaskResponse> Tasks { get; } = new();
}
