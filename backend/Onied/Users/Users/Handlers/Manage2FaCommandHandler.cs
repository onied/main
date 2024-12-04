using MediatR;
using Users.Commands;
using Users.Services.UsersService;

namespace Users.Handlers;

public class Manage2FaCommandHandler(IUsersService usersService) : IRequestHandler<Manage2FaCommand, IResult>
{
    public async Task<IResult> Handle(Manage2FaCommand request, CancellationToken cancellationToken)
    {
        return await usersService.Manage2Fa(request.TwoFactorRequest, request.User);
    }
}
