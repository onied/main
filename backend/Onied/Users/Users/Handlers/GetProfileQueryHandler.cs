using MediatR;
using Users.Commands;
using Users.Queries;
using Users.Services.ProfileService;

namespace Users.Handlers;

public class GetProfileQueryHandler(IProfileService profileService) : IRequestHandler<GetProfileQuery, IResult>
{
    public async Task<IResult> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        return await profileService.Get(request.User);
    }
}
