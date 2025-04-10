using Courses.Services.Abstractions;
using MassTransit;
using MassTransit.Data.Messages;

namespace Courses.Services.Consumers;

public class CourseCreateFailedConsumer(ICourseRepository courseRepository, ILogger<CourseCreateFailed> logger) : IConsumer<CourseCreateFailed>
{
    public async Task Consume(ConsumeContext<CourseCreateFailed> context)
    {
        logger.LogWarning("Failed to create course with id {0}: {1}", context.Message.Id, context.Message.ErrorMessage);
        await courseRepository.DeleteCourseAsync(context.Message.Id);
    }
}
