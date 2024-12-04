using MediatR;

namespace Support.Queries;

public record GetChatByIdQuery(Guid ChatId, Guid? UserId) : IRequest<IResult>;
