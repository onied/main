using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Users.Commands;
using Users.Services.UsersService;

namespace Users.Handlers;

public class LoginCommandHandler(IUsersService usersService) : IRequestHandler<LoginCommand, IResult>
{
    public async Task<IResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return await usersService.Login(request.LoginRequest);
    }
}
