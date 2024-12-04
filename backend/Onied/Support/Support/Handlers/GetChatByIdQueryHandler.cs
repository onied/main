using MediatR;
using Support.Abstractions;
using Support.Queries;

namespace Support.Handlers;

public class GetChatByIdQueryHandler(
    IChatService chatService)
    : IRequestHandler<GetChatByIdQuery, IResult>
{
    public async Task<IResult> Handle(
        GetChatByIdQuery request,
        CancellationToken cancellationToken)
        => Results.Ok(await chatService.GetChatById(request.ChatId, request.UserId));
}
