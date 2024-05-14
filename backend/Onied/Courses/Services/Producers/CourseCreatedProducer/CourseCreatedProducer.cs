using AutoMapper;
using Courses.Models;
using MassTransit;
using MassTransit.Data.Messages;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services.Producers.CourseCreatedProducer;

public class CourseCreatedProducer(
    ILogger<CourseCreatedProducer> logger,
    IMapper mapper,
    IPublishEndpoint publishEndpoint) : ICourseCreatedProducer
{
    public async Task PublishAsync(Course course)
    {
        var courseCreated = mapper.Map<CourseCreated>(course);
        await publishEndpoint.Publish(courseCreated);
        logger.LogInformation("Published created course(id={courseId})", course.Id);
    }
}
