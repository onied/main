using MediatR;

namespace Courses.Queries;

public record GetCoursesQuery(Guid UserId) : IRequest<IResult>;
