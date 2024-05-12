using AutoMapper;
using MassTransit;
using MassTransit.Data.Messages;
using Microsoft.AspNetCore.Identity;
using Users.Data.Entities;

namespace Users.Services.UserCreatedProducer;

public class UserCreatedProducer(
    ILogger<UserCreatedProducer> logger,
    IMapper mapper,
    IPublishEndpoint publishEndpoint,
    UserManager<AppUser> userManager) : IUserCreatedProducer
{
    public async Task PublishAsync(AppUser user)
    {
        if (!Guid.TryParse(await userManager.GetUserIdAsync(user), out var userId))
        {
            logger.LogError("Error when publishing User({user}): Invalid id format", user);
            return;
        }
        user.Id = userId.ToString();

        var userCreated = mapper.Map<UserCreated>(user);
        await publishEndpoint.Publish(userCreated);
        logger.LogInformation("Published User(id={userId})", userId);
    }
}
