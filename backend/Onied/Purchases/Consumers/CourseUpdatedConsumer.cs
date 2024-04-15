using AutoMapper;
using MassTransit;
using MassTransit.Data.Messages;
using Purchases.Data.Abstractions;
using Purchases.Data.Models;

namespace Purchases.Consumers;

public class CourseUpdatedConsumer(
    ILogger<CourseUpdatedConsumer> logger,
    IMapper mapper,
    ICourseRepository courseRepository) : IConsumer<CourseUpdated>
{
    public async Task Consume(ConsumeContext<CourseUpdated> context)
    {
        var courseUpdateInfo = mapper.Map<Course>(context.Message);
        logger.LogInformation("Trying to create course(id={courseId}) in database", courseUpdateInfo.Id);

        var course = await courseRepository.GetAsync(courseUpdateInfo.Id);
        if (course is null)
        {
            logger.LogError("Error while updating course(id={courseId}) in database", courseUpdateInfo.Id);
            return;
        }

        course.Title = courseUpdateInfo.Title;
        course.Price = courseUpdateInfo.Price;
        course.HasCertificates = courseUpdateInfo.HasCertificates;
        await courseRepository.UpdateAsync(course);
        logger.LogInformation("Created User profile(id={courseId}) in database", courseUpdateInfo.Id);
    }
}
