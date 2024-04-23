using Subscription = Purchases.Data.Models.Subscription;

namespace Purchases.Services.Abstractions;

public interface ISubscriptionChangedProducer
{
    public Task PublishAsync(Subscription subscription, Guid userId);
}
