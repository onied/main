using MediatR;

namespace Courses.Commands;

public record CreateCourseCommand(string? UserId) : IRequest<IResult>;
