using MediatR;
using Support.Abstractions;
using Support.Queries;

namespace Support.Handlers;

public class GetOpenChatsQueryHandler(
    ISupportService supportService)
    : IRequestHandler<GetOpenChatsQuery, IResult>
{
    public async Task<IResult> Handle(
        GetOpenChatsQuery request,
        CancellationToken cancellationToken)
        => Results.Ok(await supportService.GetOpenChats(request.UserId));
}
