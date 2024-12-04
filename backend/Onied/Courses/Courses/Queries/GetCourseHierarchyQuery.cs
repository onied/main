using MediatR;

namespace Courses.Queries;

public record GetCourseHierarchyQuery(int Id, Guid UserId, string? Role) : IRequest<IResult>;
