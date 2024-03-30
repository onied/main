namespace Users.Services.ProfileProducer;

public interface IProfileProducer
{
    public Task PublishProfileUpdatedAsync(AppUser user);
    public Task PublishProfilePhotoUpdatedAsync(AppUser user);
}
