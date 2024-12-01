using MediatR;

namespace Courses.Queries;

public record GetEditTaskBlockQuery(int Id, int BlockId, Guid UserId, string? Role) : IRequest<IResult>;
