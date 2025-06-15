using MediatR;

namespace Courses.Commands;

public record LikeCourseCommand(int Id, Guid UserId, bool IsLike) : IRequest<IResult>;
