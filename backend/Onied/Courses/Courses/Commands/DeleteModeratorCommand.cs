using MediatR;

namespace Courses.Commands;

public record DeleteModeratorCommand(int CourseId, Guid StudentId, Guid UserId) : IRequest<IResult>;
