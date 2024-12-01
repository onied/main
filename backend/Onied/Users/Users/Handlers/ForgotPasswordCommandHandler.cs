using MediatR;
using Users.Commands;
using Users.Services.UsersService;

namespace Users.Handlers;

public class ForgotPasswordCommandHandler(IUsersService usersService) : IRequestHandler<ForgotPasswordCommand, IResult>
{
    public async Task<IResult> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        return await usersService.ForgotPassword(request.ForgotPasswordRequest);
    }
}
