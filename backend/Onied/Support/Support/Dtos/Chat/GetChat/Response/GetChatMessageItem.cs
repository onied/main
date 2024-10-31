namespace Support.Dtos.Chat.GetChat.Response;

public class GetChatMessageItem
{
    public Guid MessageId { get; set; }
    public int? SupportNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Message { get; set; } = null!;
    public bool IsSystem { get; set; }
    public DateTime? ReadAt { get; set; }
}
