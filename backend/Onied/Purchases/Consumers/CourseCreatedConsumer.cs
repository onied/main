using AutoMapper;
using MassTransit;
using MassTransit.Data.Messages;
using Purchases.Data.Abstractions;
using Purchases.Data.Models;

namespace Purchases.Consumers;

public class CourseCreatedConsumer(
    ILogger<CourseCreatedConsumer> logger,
    IMapper mapper,
    ICourseRepository courseRepository) : IConsumer<CourseCreated>
{
    public async Task Consume(ConsumeContext<CourseCreated> context)
    {
        var course = mapper.Map<Course>(context.Message);
        logger.LogInformation("Trying to create course(id={courseId}) in database", course.Id);
        await courseRepository.AddAsync(course);
        logger.LogInformation("Created course(id={courseId}) in database", course.Id);
    }
}
