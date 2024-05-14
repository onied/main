using AutoMapper;
using MassTransit;
using MassTransit.Data.Messages;
using Purchases.Data.Abstractions;
using Purchases.Data.Models;
using Purchases.Data.Models.PurchaseDetails;

namespace Purchases.Services.Consumers;

public class CourseCompletedConsumer(
    ILogger<CourseCreatedConsumer> logger,
    IMapper mapper,
    IUserRepository userRepository,
    ICourseRepository courseRepository,
    IUserCourseInfoRepository userCourseInfoRepository) : IConsumer<CourseCompleted>
{
    public async Task Consume(ConsumeContext<CourseCompleted> context)
    {
        var uci = mapper.Map<UserCourseInfo>(context.Message);
        logger.LogInformation(
            "Saving course completed info(userId={userId} courseId={courseId})",
            uci.CourseId, uci.UserId);

        var course = await courseRepository.GetAsync(uci.CourseId);
        var user = await userRepository.GetAsync(uci.UserId, true);

        if (course is null
            || user is null
            || (course.Price > 0
                && user.Purchases.SingleOrDefault(p
                    => (p.PurchaseDetails as CoursePurchaseDetails)?.CourseId == uci.CourseId) is null)
            || await userCourseInfoRepository.GetAsync(uci.UserId, uci.CourseId) is not null)
        {
            logger.LogError(
                "Error occured while saving course completed info(userId={userId} courseId={courseId})",
                uci.CourseId, uci.UserId);
        }
        else
        {
            await userCourseInfoRepository.AddAsync(uci);
            logger.LogInformation("Saved course completed info(userId={userId} courseId={courseId})",
                uci.CourseId, uci.UserId);
        }
    }
}
