using System.ComponentModel.DataAnnotations.Schema;

namespace Courses.Data.Models;

[Table("course_user")]
public class UserCourseInfo
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public string? Token { get; set; }
}
