using MediatR;

namespace Courses.Commands;

public record AddModeratorCommand(int CourseId, Guid StudentId, Guid UserId) : IRequest<IResult>;
