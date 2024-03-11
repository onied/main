using Courses.Models;

namespace Courses.Dtos;

public class TasksBlockDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public BlockType BlockType { get; set; }
    public bool IsCompleted { get; set; }
    public List<TaskDto> Tasks { get; } = new();
}
