using MediatR;
using Support.Abstractions;
using Support.Queries;

namespace Support.Handlers;

public class GetUserChatQueryHandler(
    IChatService chatService)
    : IRequestHandler<GetUserChatQuery, IResult>
{
    public async Task<IResult> Handle(
        GetUserChatQuery request,
        CancellationToken cancellationToken)
        => Results.Ok(await chatService.GetUserChat(request.UserId));
}
