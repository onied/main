using Support.Abstractions;
using Support.Data.Abstractions;
using Support.Data.Models;
using Support.Exceptions;

namespace Support.Services;

public class AuthorizationSupportUserService(
    ISupportUserRepository supportUserRepository,
    ILogger<AuthorizationSupportUserService> logger
    ) : IAuthorizationSupportUserService
{
    public async Task<SupportUser> AuthorizeSupportUser(Guid? userId)
    {
        if (userId is null)
        {
            logger.LogWarning("Attempted to authorize user but userId is null");
            throw new BadRequestException("userId query parameter cannot be null");
        }

        var supportUser = await supportUserRepository.GetAsync((Guid)userId);
        if (supportUser is null)
        {
            logger.LogWarning("User authorization failed for userId: {UserId}. The user is not a support staff member.", userId);
            throw new ForbidException("The user with the given userId is not a support staff member");
        }

        logger.LogInformation("User {userId} authorized successfully", userId);
        return supportUser;
    }
}
