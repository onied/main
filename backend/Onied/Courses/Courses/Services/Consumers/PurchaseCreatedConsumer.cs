using Courses.Data.Models;
using Courses.Services.Abstractions;
using MassTransit;
using MassTransit.Data.Enums;
using MassTransit.Data.Messages;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services.Consumers;

public class PurchaseCreatedConsumer(
    ILogger<PurchaseCreatedConsumer> logger,
    IUserCourseInfoRepository userCourseInfoRepository) : IConsumer<PurchaseCreated>
{
    public async Task Consume(ConsumeContext<PurchaseCreated> context)
    {
        logger.LogInformation("Processing new PurchaseCreated");
        if (context.Message.PurchaseType is PurchaseType.Course)
        {
            await ConsumeCoursePurchase(context);
        }
    }

    private async Task ConsumeCoursePurchase(ConsumeContext<PurchaseCreated> context)
    {
        var userCourseInfo = new UserCourseInfo()
        {
            UserId = context.Message.UserId,
            CourseId = context.Message.CourseId!.Value,
            Token = context.Message.Token
        };
        try
        {
            await userCourseInfoRepository.AddUserCourseInfoAsync(userCourseInfo);
            await context.Publish(new PurchaseCreatedCourses(context.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to add user course info");
            await context.Publish(new PurchaseCreateFailed(context.Message.Id, context.Message.Token, ex.Message));
        }
    }
}
