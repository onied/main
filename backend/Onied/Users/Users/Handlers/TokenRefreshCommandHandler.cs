using MediatR;
using Users.Commands;
using Users.Services.UsersService;

namespace Users.Handlers;

public class TokenRefreshCommandHandler(IUsersService usersService) : IRequestHandler<TokenRefreshCommand, IResult>
{
    public async Task<IResult> Handle(TokenRefreshCommand request, CancellationToken cancellationToken)
    {
        return await usersService.Refresh(request.RefreshRequest);
    }
}
