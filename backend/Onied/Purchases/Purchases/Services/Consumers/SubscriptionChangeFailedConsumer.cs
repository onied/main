using MassTransit;
using MassTransit.Data.Messages;
using Purchases.Data.Abstractions;

namespace Purchases.Services.Consumers;

public class SubscriptionChangeFailedConsumer(IPurchaseUnitOfWork unitOfWork, ILogger<SubscriptionChangeFailedConsumer> logger)
    : IConsumer<SubscriptionChangeFailed>
{
    public async Task Consume(ConsumeContext<SubscriptionChangeFailed> context)
    {
        logger.LogWarning("Failed to change subscription with UserId {0}: {1}", context.Message.UserId, context.Message.ErrorMessage);
        await unitOfWork.BeginTransactionAsync();
        var user = (await unitOfWork.Users.GetAsync(context.Message.UserId))!;
        user.SubscriptionId = context.Message.OldSubscriptionId;
        await unitOfWork.Users.UpdateAsync(user);
        await unitOfWork.Purchases.RemoveAsyncById(context.Message.PurchaseId);
        await unitOfWork.CommitTransactionAsync();
    }
}
