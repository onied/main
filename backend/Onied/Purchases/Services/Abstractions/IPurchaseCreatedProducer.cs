using Purchases.Data.Models;

namespace Purchases.Services.Abstractions;

public interface IPurchaseCreatedProducer
{
    Task PublishAsync(Purchase purchase);
}
