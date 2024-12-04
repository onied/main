using Courses.Commands;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class AddModeratorCommandHandler(ICourseManagementService courseManagementService)
    : IRequestHandler<AddModeratorCommand, IResult>
{
    public async Task<IResult> Handle(
        AddModeratorCommand request,
        CancellationToken cancellationToken
    )
    {
        return await courseManagementService.AddModerator(
            request.CourseId,
            request.StudentId,
            request.UserId
        );
    }
}
