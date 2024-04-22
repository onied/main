using AutoMapper;
using MassTransit;
using MassTransit.Courier.Contracts;
using MassTransit.Data.Messages;
using Purchases.Services.Abstractions;

namespace Purchases.Services.Producers;

public class SubscriptionChangedProducer(
    ILogger<PurchaseCreatedProducer> logger,
    IMapper mapper,
    IPublishEndpoint publishEndpoint) : ISubscriptionChangedProducer
{
    public async Task PublishAsync(Subscription subscription, Guid userId)
    {
        var subscriptionChanged = mapper.Map<SubscriptionChanged>(subscription);
        subscriptionChanged.UserId = userId;
        await publishEndpoint.Publish(subscriptionChanged);
    }
}
