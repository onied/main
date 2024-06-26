using Users.Data.Entities;

namespace Users.Services.UserCreatedProducer;

public interface IUserCreatedProducer
{
    public Task PublishAsync(AppUser user);
}
