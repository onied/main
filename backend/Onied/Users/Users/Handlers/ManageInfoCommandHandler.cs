using MediatR;
using Users.Commands;
using Users.Services.UsersService;

namespace Users.Handlers;

public class ManageInfoCommandHandler(IUsersService usersService) : IRequestHandler<ManageInfoCommand, IResult>
{
    public async Task<IResult> Handle(ManageInfoCommand request, CancellationToken cancellationToken)
    {
        return await usersService.PostInfo(request.InfoRequest, request.HttpContext);
    }
}
