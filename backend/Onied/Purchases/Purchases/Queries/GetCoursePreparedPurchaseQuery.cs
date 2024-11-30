using MediatR;

namespace Purchases.Queries;

public record GetCoursePreparedPurchaseQuery(int CourseId) : IRequest<IResult>;
