using Courses.Commands;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class DeleteModeratorCommandHandler(ICourseManagementService courseManagementService)
    : IRequestHandler<DeleteModeratorCommand, IResult>
{
    public async Task<IResult> Handle(
        DeleteModeratorCommand request,
        CancellationToken cancellationToken
    )
    {
        return await courseManagementService.DeleteModerator(
            request.CourseId,
            request.StudentId,
            request.UserId
        );
    }
}
