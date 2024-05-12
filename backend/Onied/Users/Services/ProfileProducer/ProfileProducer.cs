using AutoMapper;
using MassTransit;
using MassTransit.Data.Messages;
using Microsoft.AspNetCore.Identity;
using Users.Data;

namespace Users.Services.ProfileProducer;

public class ProfileProducer(
    ILogger<ProfileProducer> logger,
    IMapper mapper,
    IPublishEndpoint publishEndpoint,
    UserManager<AppUser> userManager) : IProfileProducer
{

    public async Task PublishProfileUpdatedAsync(AppUser user)
    {
        if (!Guid.TryParse(await userManager.GetUserIdAsync(user), out var userId))
        {
            logger.LogError("Error when publishing User({user}): Invalid id format", user);
            return;
        }
        user.Id = userId.ToString();

        var profileUpdated = mapper.Map<ProfileUpdated>(user);
        await publishEndpoint.Publish(profileUpdated);
        logger.LogInformation("Published ProfileUpdated(id={userId})", userId);
    }

    public async Task PublishProfilePhotoUpdatedAsync(AppUser user)
    {
        if (!Guid.TryParse(await userManager.GetUserIdAsync(user), out var userId))
        {
            logger.LogError("Error when publishing User(id={userId}): Invalid id format", userId);
            return;
        }
        user.Id = userId.ToString();

        var profilePhotoUpdated = mapper.Map<ProfilePhotoUpdated>(user);
        await publishEndpoint.Publish(profilePhotoUpdated);
        logger.LogInformation("Published ProfileProtoUpdated(id={userId})", userId);
    }
}
