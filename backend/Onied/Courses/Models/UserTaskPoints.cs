namespace Courses.Dtos;

public class UserTaskPoints
{
    public int Id { get; set; }
    
    // public int UserId { get; set; }
    
    // public User User { get; set; }
    
    public int TaskId { get; set; }
    public Task Task { get; set; }
    
    public int Points { get; set; }
}