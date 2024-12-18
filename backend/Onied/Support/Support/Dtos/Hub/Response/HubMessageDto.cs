namespace Support.Dtos.Hub.Response;

public class HubMessageDto
{
    public Guid MessageId { get; set; }
    public int? SupportNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Message { get; set; } = null!;
    public bool IsSystem { get; set; }

    public ICollection<HubMessageFileDto> Files { get; set; } = new List<HubMessageFileDto>();
}
