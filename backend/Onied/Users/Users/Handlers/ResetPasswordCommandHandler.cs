using MediatR;
using Users.Commands;
using Users.Services.UsersService;

namespace Users.Handlers;

public class ResetPasswordCommandHandler(IUsersService usersService) : IRequestHandler<ResetPasswordCommand, IResult>
{
    public async Task<IResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        return await usersService.ResetPassword(request.ResetPasswordRequest);
    }
}
