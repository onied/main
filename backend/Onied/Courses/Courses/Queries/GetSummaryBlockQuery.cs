using MediatR;

namespace Courses.Queries;

public record GetSummaryBlockQuery(int Id, int BlockId, Guid UserId, string? Role) : IRequest<IResult>;
