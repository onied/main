using Courses.Commands;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class EditCheckedTaskBlockHandler(ICheckTaskManagementService checkTaskManagementService)
    : IRequestHandler<EditCheckedTaskBlockCommand, IResult>
{
    public async Task<IResult> Handle(
        EditCheckedTaskBlockCommand request,
        CancellationToken cancellationToken
    )
    {
        return await checkTaskManagementService.CheckTaskBlock(
            request.CourseId,
            request.BlockId,
            request.UserId,
            request.Role,
            request.InputsDto
        );
    }
}
