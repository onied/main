using MassTransit.Courier.Contracts;

namespace Purchases.Services.Abstractions;

public interface ISubscriptionChangedProducer
{
    public Task PublishAsync(Subscription subscription, Guid userId);
}
