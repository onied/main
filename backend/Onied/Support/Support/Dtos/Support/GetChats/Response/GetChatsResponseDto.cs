namespace Support.Dtos.Support.GetChats.Response;

public class GetChatsResponseDto
{
    public Guid ChatId { get; set; }
    public GetChatsMessageItem LastMessage { get; set; } = null!;
}
