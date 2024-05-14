namespace MassTransit.Data.Messages;

public record CourseCompleted(Guid UserId, int CourseId);
