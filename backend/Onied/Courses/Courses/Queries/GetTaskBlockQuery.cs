using MediatR;

namespace Courses.Queries;

public record GetTaskBlockQuery(int Id, int BlockId, Guid UserId, string? Role) : IRequest<IResult>;
