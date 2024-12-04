using MediatR;

namespace Purchases.Queries;

public record GetSubscriptionsByUserQuery(Guid UserId) : IRequest<IResult>;
