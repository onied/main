namespace Support.Events.Messages;

public record MarkMessageAsRead(Guid SenderId, Guid MessageId);
