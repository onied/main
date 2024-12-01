using MediatR;

namespace Courses.Queries;

public record GetStudentsQuery(int CourseId, Guid UserId) : IRequest<IResult>;
