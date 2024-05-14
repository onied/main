using AutoMapper;
using Courses.Data.Models;
using MassTransit;
using MassTransit.Data.Messages;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services.Producers.CourseUpdatedProducer;

public class CourseUpdatedProducer(
    ILogger<CourseUpdatedProducer> logger,
    IMapper mapper,
    IPublishEndpoint publishEndpoint) : ICourseUpdatedProducer
{
    public async Task PublishAsync(Course course)
    {
        var courseUpdated = mapper.Map<CourseUpdated>(course);
        await publishEndpoint.Publish(courseUpdated);
        logger.LogInformation("Published updated course(id={courseId})", course.Id);
    }
}
