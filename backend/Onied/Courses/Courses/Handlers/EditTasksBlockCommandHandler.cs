using Courses.Commands;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class EditTasksBlockCommandHandler(ICourseManagementService courseManagementService)
    : IRequestHandler<EditTasksBlockCommand, IResult>
{
    public async Task<IResult> Handle(EditTasksBlockCommand request, CancellationToken cancellationToken)
    {
        return await courseManagementService.EditTasksBlock(request.Id, request.BlockId, request.TasksBlockRequest);
    }
}
