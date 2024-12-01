using MediatR;
using Support.Abstractions;
using Support.Queries;

namespace Support.Handlers;

public class GetProfileQueryHandler(
    ISupportService supportService)
    : IRequestHandler<GetProfileQuery, IResult>
{
    public async Task<IResult> Handle(
        GetProfileQuery request,
        CancellationToken cancellationToken)
        => Results.Ok(await supportService.GetProfile(request.UserId));
}
