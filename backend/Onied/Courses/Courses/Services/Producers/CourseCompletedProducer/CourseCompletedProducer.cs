using MassTransit;
using MassTransit.Data.Messages;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services.Producers.CourseCompletedProducer;

public class CourseCompletedProducer(
    ILogger<CourseCompletedProducer> logger,
    IPublishEndpoint publishEndpoint) : ICourseCompletedProducer
{
    public async Task PublishAsync(CourseCompleted courseCompleted)
    {
        await publishEndpoint.Publish(courseCompleted);
        logger.LogInformation("Sent course completed(userId={userId}, courseId={CourseId}",
            courseCompleted.UserId,
            courseCompleted.CourseId);
    }
}
