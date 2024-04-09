using AutoMapper;
using Courses.Models;
using Courses.Services.Abstractions;
using MassTransit;
using MassTransit.Data.Messages;
using Task = System.Threading.Tasks.Task;

namespace Courses.Services.Consumers;

public class UserCreatedConsumer(
    ILogger<UserCreatedConsumer> logger,
    IMapper mapper,
    IUserRepository userRepository) : IConsumer<UserCreated>
{
    public async Task Consume(ConsumeContext<UserCreated> context)
    {
        var user = mapper.Map<User>(context.Message);
        logger.LogInformation("Trying to create User profile(id={userId}) photo in database", user.Id);
        await userRepository.AddUserAsync(user);
        logger.LogInformation("Created User profile(id={userId}) in database", user.Id);
    }
}
