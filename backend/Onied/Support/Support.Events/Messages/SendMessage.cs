namespace Support.Events.Messages;

public record SendMessage(Guid SenderId, string MessageContent);
