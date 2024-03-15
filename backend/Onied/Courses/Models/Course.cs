using System.ComponentModel.DataAnnotations;

namespace Courses.Models;

public class Course
{
    public int Id { get; set; }

    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    [MinLength(1)]
    [MaxLength(200)]
    public string Title { get; set; } = null!;

    [Url]
    [MaxLength(2048)]
    public string PictureHref { get; set; } = null!;

    [MinLength(1)]
    [MaxLength(15000)]
    public string Description { get; set; } = null!;

    [Range(0, 35000)]
    public int HoursCount { get; set; }

    [Range(0, 1000000)]
    public int PriceRubles { get; set; }

    public bool IsArchived { get; set; }

    public bool IsGlowing { get; set; }

    public bool HasCertificates { get; set; }

    public bool IsProgramVisible { get; set; }

    public ICollection<Module> Modules { get; } = new List<Module>();
}
