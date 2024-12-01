using Courses.Commands;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class AddBlockCommandHandler(ICourseManagementService courseManagementService) : IRequestHandler<AddBlockCommand, IResult>
{
    public async Task<IResult> Handle(AddBlockCommand request, CancellationToken cancellationToken)
    {
        return await courseManagementService.AddBlock(request.Id, request.ModuleId, request.AddBlockRequest.BlockType);
    }
}
