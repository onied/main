using Courses.Models;
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
        await userCourseInfoRepository.AddUserCourseInfoAsync(userCourseInfo);
    }
}
