namespace Support.Dtos.Chat.GetChat.Response;

public class GetChatResponseDto
{
    public int? SupportNumber { get; set; }
    public Guid? CurrentSessionId { get; set; }
    public List<GetChatMessageItem> Messages { get; set; } = null!;
}
