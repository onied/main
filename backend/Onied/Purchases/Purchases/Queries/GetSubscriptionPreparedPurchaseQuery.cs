using MediatR;

namespace Purchases.Queries;

public record GetSubscriptionPreparedPurchaseQuery(int SubscriptionId) : IRequest<IResult>;
