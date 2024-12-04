using Courses.Commands;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class EditVideoBlockCommandHandler(ICourseManagementService courseManagementService)
    : IRequestHandler<EditVideoBlockCommand, IResult>
{
    public async Task<IResult> Handle(EditVideoBlockCommand request, CancellationToken cancellationToken)
    {
        return await courseManagementService.EditVideoBlock(request.Id, request.BlockId, request.VideoBlockResponse);
    }
}
