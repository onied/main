namespace Support.Events.Messages;

public record CloseChat(Guid SenderId, Guid ChatId);
