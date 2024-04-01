using AutoMapper;
using Courses.Enums;
using MassTransit;
using MassTransit.Data.Messages;

namespace Courses.Services;

public class ProfileUpdatedConsumer(
    ILogger<ProfileUpdatedConsumer> logger,
    IMapper mapper,
    IUserRepository userRepository) : IConsumer<ProfileUpdated>
{

    public async Task Consume(ConsumeContext<ProfileUpdated> context)
    {
        var message = context.Message;

        logger.LogInformation("Trying to update User profile(id={userId}) in database", message.Id);
        var user = await userRepository.GetUserAsync(message.Id);
        if (user is null)
        {
            logger.LogError("User profile with id={userId} not found", message.Id);
            return;
        }

        user.FirstName = message.FirstName;
        user.LastName = message.LastName;
        user.Gender = message.Gender as Gender?;
        await userRepository.UpdateUserAsync(user);
        logger.LogInformation("User profile Profile(id{userId}) in database", user.Id);
    }
}
