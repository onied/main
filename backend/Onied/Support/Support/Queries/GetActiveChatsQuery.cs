using MediatR;

namespace Support.Queries;

public record GetActiveChatsQuery(Guid? UserId) : IRequest<IResult>;
