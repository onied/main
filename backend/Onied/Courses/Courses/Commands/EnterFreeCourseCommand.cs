using MediatR;

namespace Courses.Commands;

public record EnterFreeCourseCommand(int Id, Guid UserId) : IRequest<IResult>;
