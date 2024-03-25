using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Courses.Enums;

namespace Courses.Models;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }

    [MinLength(1)]
    [MaxLength(50)]
    public string FirstName { get; set; } = null!;

    [MinLength(0)]
    [MaxLength(50)]
    public string LastName { get; set; } = null!;

    public Gender? Gender { get; set; }

    [Url]
    [MaxLength(2048)]
    public string? AvatarHref { get; set; }

    public ICollection<Course> Courses { get; } = new List<Course>();

    public ICollection<Course> TeachingCourses { get; } = new List<Course>();
}
