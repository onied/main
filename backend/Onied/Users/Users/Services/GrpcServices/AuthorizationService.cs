using System.Security.Claims;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using UsersGrpc;

namespace Users.Services.GrpcServices;

public class AuthorizationService : UsersGrpc.AuthorizationService.AuthorizationServiceBase
{
    public override Task<UserIdResponse> GetCurrentUserId(Empty request, ServerCallContext context)
    {
        var userId = context.GetHttpContext().User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)
            ?.Value ?? "";
        return Task.FromResult(new UserIdResponse { UserId = userId });
    }
}
