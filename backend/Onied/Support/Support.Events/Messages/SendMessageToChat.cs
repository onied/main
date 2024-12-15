using Support.Events.Dtos;

namespace Support.Events.Messages;

public record SendMessageToChat(
    Guid SenderId,
    Guid ChatId,
    string MessageContent,
    IEnumerable<SendMessageFileDto> Files);
