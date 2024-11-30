using MediatR;

namespace Support.Queries;

public record GetOpenChatsQuery(Guid? UserId) : IRequest<IResult>;
