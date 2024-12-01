using MediatR;

namespace Purchases.Queries;

public record GetActiveSubscriptionQuery(Guid UserId) : IRequest<IResult>;
