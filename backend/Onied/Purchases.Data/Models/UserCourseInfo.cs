namespace Purchases.Data.Models;

public class UserCourseInfo
{
    public Guid UserId { get; set; }
    public int CourseId { get; set; }
    public bool IsCompleted { get; set; }
}
