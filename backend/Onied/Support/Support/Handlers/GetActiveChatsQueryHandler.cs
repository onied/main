using MediatR;
using Support.Abstractions;
using Support.Queries;

namespace Support.Handlers;

public class GetActiveChatsQueryHandler(
    ISupportService supportService)
    : IRequestHandler<GetActiveChatsQuery, IResult>
{
    public async Task<IResult> Handle(
        GetActiveChatsQuery request,
        CancellationToken cancellationToken)
        => Results.Ok(await supportService.GetActiveChats(request.UserId));
}
