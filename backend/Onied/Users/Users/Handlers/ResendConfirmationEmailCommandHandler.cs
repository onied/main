using MediatR;
using Users.Commands;
using Users.Services.UsersService;

namespace Users.Handlers;

public class ResendConfirmationEmailCommandHandler(IUsersService usersService)
    : IRequestHandler<ResendConfirmationEmailCommand, IResult>
{
    public async Task<IResult> Handle(ResendConfirmationEmailCommand request, CancellationToken cancellationToken)
    {
        return await usersService.ResendConfirmationEmail(request.ResendConfirmationEmailRequest, request.HttpContext);
    }
}
