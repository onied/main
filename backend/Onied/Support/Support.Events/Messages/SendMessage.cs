using Support.Events.Dtos;

namespace Support.Events.Messages;

public record SendMessage(Guid SenderId, string MessageContent, IEnumerable<SendMessageFileDto> Files);
