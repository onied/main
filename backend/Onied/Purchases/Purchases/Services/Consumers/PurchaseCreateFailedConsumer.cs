using MassTransit;
using MassTransit.Data.Messages;
using Purchases.Data.Abstractions;

namespace Purchases.Services.Consumers;

public class PurchaseCreateFailedConsumer(IPurchaseRepository purchaseRepository, ILogger<PurchaseCreateFailedConsumer> logger)
    : IConsumer<PurchaseCreateFailed>
{
    public async Task Consume(ConsumeContext<PurchaseCreateFailed> context)
    {
        logger.LogWarning("Failed to create purchase with id {0}: {1}", context.Message.Id, context.Message.ErrorMessage);
        await purchaseRepository.RemoveAsyncById(context.Message.Id);
    }
}
