using MediatR;

namespace Courses.Queries;

public record GetTaskPointsStoredQuery(int CourseId, int BlockId, Guid UserId, string? Role)
    : IRequest<IResult>;
