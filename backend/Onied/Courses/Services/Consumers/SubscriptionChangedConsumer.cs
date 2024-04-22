using AutoMapper;
using Courses.Services.Abstractions;
using MassTransit;
using MassTransit.Data.Messages;

namespace Courses.Services.Consumers;

public class SubscriptionChangedConsumer(
    ILogger<SubscriptionChangedConsumer> logger,
    ISubscriptionManagementService
    IMapper mapper) : IConsumer<SubscriptionChanged>
{
    public Task Consume(ConsumeContext<SubscriptionChanged> context)
    {
        var message = context.Message;


        logger.LogInformation("Trying to update process subscription change (id={userId}) photo in database", message.Id);
    }
}
