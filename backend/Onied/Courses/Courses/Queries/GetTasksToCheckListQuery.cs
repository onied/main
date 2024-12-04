using MediatR;

namespace Courses.Queries;

public record GetTasksToCheckListQuery(Guid UserId) : IRequest<IResult>;
