using MediatR;

namespace Courses.Queries;

public record GetModeratedCoursesQuery(Guid UserId) : IRequest<IResult>;
