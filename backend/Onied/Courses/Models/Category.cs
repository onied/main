using System.ComponentModel.DataAnnotations;

namespace Courses.Models;

public class Category
{
    public int Id { get; set; }

    [MinLength(1)]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

    public ICollection<Course> Courses { get; } = new List<Course>();
}
