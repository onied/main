using MediatR;
using Users.Commands;
using Users.Services.UsersService;

namespace Users.Handlers;

public class RegisterCommandHandler(IUsersService usersService) : IRequestHandler<RegisterCommand, IResult>
{
    public async Task<IResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        return await usersService.Register(request.RegisterUserRequest, request.HttpContext);
    }
}
