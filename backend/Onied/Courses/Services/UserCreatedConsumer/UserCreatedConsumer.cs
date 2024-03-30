using AutoMapper;
using Courses.Models;
using MassTransit;
using MassTransit.Data.Messages;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services.UserCreatedConsumer;

public class UserCreatedConsumer(
    ILogger<UserCreatedConsumer> logger,
    IMapper mapper,
    IUserRepository userRepository) : IConsumer<UserCreated>
{
    public async Task Consume(ConsumeContext<UserCreated> context)
    {
        var user = mapper.Map<User>(context.Message);
        await userRepository.SaveUserAsync(user);
        logger.LogInformation("Created User({user}) in database", user);
    }
}
