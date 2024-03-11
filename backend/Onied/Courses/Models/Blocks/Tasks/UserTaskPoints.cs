namespace Courses.Models.Blocks.Tasks;

public class UserTaskPoints
{
    public int UserId { get; set; } = -1;
    public int TaskId { get; set; }
    // при сохранении в БД связка (UserId, TaskId) дожлна быть primary key
    
    // public User User { get; set; }
    // public Task Task { get; set; }
    // пока не вижу смысла
    
    public int Points { get; set; }
}