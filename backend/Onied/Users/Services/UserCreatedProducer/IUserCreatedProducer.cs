using Users.Data;

namespace Users.Services.UserCreatedProducer;

public interface IUserCreatedProducer
{
    public Task PublishAsync(AppUser user);
}
