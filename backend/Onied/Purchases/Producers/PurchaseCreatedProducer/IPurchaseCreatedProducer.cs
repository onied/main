using Purchases.Data.Models;

namespace Purchases.Producers.PurchaseCreatedProducer;

public interface IPurchaseCreatedProducer
{
    Task PublishAsync(Purchase purchase);
}
