namespace Courses.Models.Blocks.Tasks.TaskUserInput;

public abstract class UserInput
{
    // public int Id { get; set; }

    public int UserId { get; set; } = -1;
    // public User User { get; set; } = null!;
    
    public int TaskId { get; set; }
    // public Task Task { get; set; } = null!;
}