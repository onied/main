using MediatR;
using Users.Queries;
using Users.Services.UsersService;

namespace Users.Handlers;

public class GetManageInfoQueryHandler(IUsersService usersService) : IRequestHandler<GetManageInfoQuery, IResult>
{
    public async Task<IResult> Handle(GetManageInfoQuery request, CancellationToken cancellationToken)
    {
        return await usersService.GetInfo(request.User);
    }
}
