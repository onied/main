namespace Purchases.Data.Models;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public bool HasCertificates { get; set; }

    public Guid AuthorId { get; set; }
    public User Author { get; set; } = null!;
}
