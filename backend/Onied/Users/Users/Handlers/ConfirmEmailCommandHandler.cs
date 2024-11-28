using MediatR;
using Users.Commands;
using Users.Services.UsersService;

namespace Users.Handlers;

public class ConfirmEmailCommandHandler(IUsersService usersService) : IRequestHandler<ConfirmEmailCommand, IResult>
{
    public async Task<IResult> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        return await usersService.ConfirmEmail(request.UserId, request.Code, request.ChangedEmail);
    }
}
