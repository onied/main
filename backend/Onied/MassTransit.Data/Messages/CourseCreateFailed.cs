namespace MassTransit.Data.Messages;

public record CourseCreateFailed(int CourseId, string ErrorMessage);
