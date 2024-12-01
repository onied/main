using MediatR;
using Users.Commands;
using Users.Services.UsersService;

namespace Users.Handlers;

public class SignInVkCommandHandler(IUsersService usersService) : IRequestHandler<SignInVkCommand, IResult>
{
    public async Task<IResult> Handle(SignInVkCommand request, CancellationToken cancellationToken)
    {
        return await usersService.SigninVk(request.OauthCodeRequest);
    }
}
