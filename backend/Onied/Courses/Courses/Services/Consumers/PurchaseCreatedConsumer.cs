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
        var message = context.Message;
        var userCourseInfo = new UserCourseInfo()
        {
            UserId = message.UserId,
            CourseId = message.CourseId!.Value,
            Token = message.Token
        };
        try
        {
            await userCourseInfoRepository.AddUserCourseInfoAsync(userCourseInfo);
            await context.Publish(new PurchaseCreatedCourses(
                message.Id,
                message.UserId,
                message.PurchaseType,
                message.CourseId,
                message.SubscriptionId,
                message.Token));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to add user course info");
            await context.Publish(new PurchaseCreateFailed(context.Message.Id, context.Message.Token, ex.Message));
        }
    }
}
