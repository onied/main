using MediatR;

namespace Support.Queries;

public record GetUserChatQuery(Guid? UserId) : IRequest<IResult>;
