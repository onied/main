using AutoMapper;
using Courses.Services.Abstractions;
using MassTransit;
using MassTransit.Data.Messages;

namespace Courses.Services.Consumers;

public class ProfilePhotoUpdatedConsumer(
    ILogger<ProfilePhotoUpdatedConsumer> logger,
    IMapper mapper,
    IUserRepository userRepository) : IConsumer<ProfilePhotoUpdated>
{

    public async Task Consume(ConsumeContext<ProfilePhotoUpdated> context)
    {
        var message = context.Message;

        logger.LogInformation("Trying to update User profile(id={userId}) photo in database", message.Id);
        var user = await userRepository.GetUserAsync(message.Id);
        if (user is null)
        {
            logger.LogError("User profile with id={userId} not found", message.Id);
            return;
        }

        user.AvatarHref = message.AvatarHref;
        await userRepository.UpdateUserAsync(user);
        logger.LogInformation("Updated User profile(id{userId}) photo in database", user.Id);
    }
}
