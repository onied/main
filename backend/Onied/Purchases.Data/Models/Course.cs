using System.ComponentModel.DataAnnotations;

namespace Purchases.Data.Models;

public class Course
{
    public int Id { get; set; }

    [MinLength(1)]
    [MaxLength(200)]
    public string Title { get; set; } = null!;

    [Range(0, 1000000)]
    public decimal Price { get; set; }
    public bool HasCertificates { get; set; }

    public Guid AuthorId { get; set; }
    public User Author { get; set; } = null!;
}
