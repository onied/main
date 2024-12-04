using MediatR;

namespace Courses.Queries;

public record GetTaskToCheckQuery(Guid UserId, Guid UserAnswerId) : IRequest<IResult>;
