using Courses.Commands;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class EditHierarchyCommandHandler(ICourseManagementService courseManagementService)
    : IRequestHandler<EditHierarchyCommand, IResult>
{
    public async Task<IResult> Handle(EditHierarchyCommand request, CancellationToken cancellationToken)
    {
        return await courseManagementService.EditHierarchy(request.Id, request.CourseResponse);
    }
}
