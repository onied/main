using System.ComponentModel.DataAnnotations;

namespace Courses.Models;

public class UserTaskPoints
{
    [Required]
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    [Required]
    public int TaskId { get; set; }
    public Task Task { get; set; } = null!;

    [Required]
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public int Points { get; set; }

    [Required]
    public UserCourseInfo UserCourseInfo { get; set; } = null!;
}
