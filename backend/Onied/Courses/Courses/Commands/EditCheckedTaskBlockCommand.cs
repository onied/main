using Courses.Dtos.CheckTasks.Request;
using MediatR;

namespace Courses.Commands;

public record EditCheckedTaskBlockCommand(
    int CourseId,
    int BlockId,
    Guid UserId,
    string? Role,
    List<UserInputRequest> InputsDto
) : IRequest<IResult>;
