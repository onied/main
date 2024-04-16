using AutoMapper;
using MassTransit;
using MassTransit.Data.Messages;
using Purchases.Data.Abstractions;
using Purchases.Data.Enums;
using Purchases.Data.Models;

namespace Purchases.Services.Consumers;

public class UserCreatedConsumer(
    ILogger<UserCreatedConsumer> logger,
    IMapper mapper,
    IUserRepository userRepository) : IConsumer<UserCreated>
{
    public async Task Consume(ConsumeContext<UserCreated> context)
    {
        var user = mapper.Map<User>(context.Message);
        user.SubscriptionId = (int)SubscriptionType.Free;
        logger.LogInformation("Trying to create User profile(id={userId}) photo in database", user.Id);
        await userRepository.AddAsync(user);
        logger.LogInformation("Created User profile(id={userId}) in database", user.Id);
    }
}
