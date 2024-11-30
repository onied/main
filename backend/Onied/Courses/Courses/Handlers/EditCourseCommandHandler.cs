using Courses.Commands;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class EditCourseCommandHandler(ICourseManagementService courseManagementService) : IRequestHandler<EditCourseCommand, IResult>
{
    public async Task<IResult> Handle(EditCourseCommand request, CancellationToken cancellationToken)
    {
        return await courseManagementService.EditCourse(request.Id, request.EditCourseRequest, request.UserId!);
    }
}
