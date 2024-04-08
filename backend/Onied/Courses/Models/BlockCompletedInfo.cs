namespace Courses.Models;

public class BlockCompletedInfo
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public int BlockId { get; set; }
    public Block Block { get; set; } = null!;
}
