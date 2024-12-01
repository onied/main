using MediatR;

namespace Notifications.Queries;

public record GetMessagesQuery(Guid UserId) : IRequest<IResult>;
