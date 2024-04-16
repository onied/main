using MassTransit;
using MassTransit.Data.Enums;
using MassTransit.Data.Messages;
using Purchases.Data.Models;
using Purchases.Data.Models.PurchaseDetails;

namespace Purchases.Producers.PurchaseCreatedProducer;

public class PurchaseCreatedProducer(
    ILogger<PurchaseCreatedProducer> logger,
    IPublishEndpoint publishEndpoint) : IPurchaseCreatedProducer
{
    public async Task PublishAsync(Purchase purchase)
    {
        var purchaseCreated = new PurchaseCreated(
            purchase.UserId,
            (PurchaseType)purchase.PurchaseDetails.PurchaseType,
            purchase.PurchaseDetails is CoursePurchaseDetails pd1
                ? pd1.CourseId
                : (purchase.PurchaseDetails as CertificatePurchaseDetails)?.CourseId,
            (purchase.PurchaseDetails as SubscriptionPurchaseDetails)?.SubscriptionId,
            purchase.Token);
        await publishEndpoint.Publish(purchaseCreated);
        logger.LogInformation("Published PurchaseCreated(purchaseId={purchaseId})", purchase.Id);
    }
}
