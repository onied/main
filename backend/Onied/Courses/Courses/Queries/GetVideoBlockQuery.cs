using MediatR;

namespace Courses.Queries;

public record GetVideoBlockQuery(int Id, int BlockId, Guid UserId, string? Role) : IRequest<IResult>;
