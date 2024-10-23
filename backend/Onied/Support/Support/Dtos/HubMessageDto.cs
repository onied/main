namespace Support.Dtos;

public class HubMessageDto
{
    public Guid MessageId { get; set; }
    public int? SupportNumbers { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Message { get; set; } = null!;
    public bool IsSystem { get; set; }
}
