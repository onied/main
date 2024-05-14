namespace MassTransit.Data.Messages;

public record CourseCreated
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
    public bool HasCertificates { get; set; }
    public Guid AuthorId { get; set; }
}
