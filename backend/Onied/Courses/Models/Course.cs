using System.ComponentModel.DataAnnotations;

namespace Courses.Models;

public class Course
{
    public int Id { get; set; }

    [MinLength(1)]
    [MaxLength(200)]
    public string Title { get; set; } = null!;

    public ICollection<Module> Modules { get; } = new List<Module>();
}