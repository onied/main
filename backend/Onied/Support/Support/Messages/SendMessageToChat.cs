namespace Support.Messages;

public record SendMessageToChat(Guid SenderId, Guid ChatId, string MessageContent);
