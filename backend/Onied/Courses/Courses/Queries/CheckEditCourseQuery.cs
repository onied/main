using MediatR;

namespace Courses.Queries;

public record CheckEditCourseQuery(int Id, string? UserId, string? Role) : IRequest<IResult>;
