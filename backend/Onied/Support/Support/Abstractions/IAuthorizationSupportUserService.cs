using Support.Data.Models;

namespace Support.Abstractions;

public interface IAuthorizationSupportUserService
{
    public Task<SupportUser> AuthorizeSupportUser(Guid? userId);
}
