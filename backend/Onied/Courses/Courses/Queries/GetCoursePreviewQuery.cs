using MediatR;

namespace Courses.Queries;

public record GetCoursePreviewQuery(int Id, Guid? UserId) : IRequest<IResult>;
