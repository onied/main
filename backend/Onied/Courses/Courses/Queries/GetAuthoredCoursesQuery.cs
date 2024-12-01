using MediatR;

namespace Courses.Queries;

public record GetAuthoredCoursesQuery(Guid UserId) : IRequest<IResult>;
