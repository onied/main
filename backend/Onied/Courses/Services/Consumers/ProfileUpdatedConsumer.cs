using AutoMapper;
using Courses.Enums;
using Courses.Services.Abstractions;
using MassTransit;
using MassTransit.Data.Messages;

namespace Courses.Services.Consumers;

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
        user.Gender = message.Gender != null ? Enum.Parse<Gender>(message.Gender.Value.ToString()) : null;
        await userRepository.UpdateUserAsync(user);
        logger.LogInformation("User profile Profile(id{userId}) in database", user.Id);
    }
}
