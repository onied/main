using System.ComponentModel.DataAnnotations;

namespace Courses.Models;

public class Author
{
    public int Id { get; set; }

    [MinLength(1)]
    [MaxLength(50)]
    public string FirstName { get; set; } = null!;

    [MinLength(0)]
    [MaxLength(50)]
    public string LastName { get; set; } = null!;

    [Url]
    [MaxLength(2048)]
    public string AvatarHref { get; set; } = null!;

    public ICollection<Course> Courses { get; } = new List<Course>();
}
