namespace Courses.Dtos.ModeratorDtos.Response;

public class StudentDto
{
    public Guid StudentId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? AvatarHref { get; set; }
    public bool IsModerator { get; set; }
}
