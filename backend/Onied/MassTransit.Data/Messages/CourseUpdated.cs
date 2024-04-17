namespace MassTransit.Data.Messages;

public class CourseUpdated
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
    public bool HasCertificates { get; set; }
}
