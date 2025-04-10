using Courses.Services.Abstractions;
using MassTransit;
using MassTransit.Data.Messages;

namespace Courses.Services.Consumers;

public class PurchaseCreateFailedConsumer(IUserCourseInfoRepository userCourseInfoRepository, ILogger<PurchaseCreateFailedConsumer> logger)
    : IConsumer<PurchaseCreateFailed>
{
    public async Task Consume(ConsumeContext<PurchaseCreateFailed> context)
    {
        logger.LogWarning("Failed to create purchase with id {0}: {1}", context.Message.Id, context.Message.ErrorMessage);
        await userCourseInfoRepository.RemoveAsyncByToken(context.Message.Token);
    }
}
