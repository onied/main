using Courses.Commands;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class DeleteBlockCommandHandler(ICourseManagementService courseManagementService)
    : IRequestHandler<DeleteBlockCommand, IResult>
{
    public async Task<IResult> Handle(DeleteBlockCommand request, CancellationToken cancellationToken)
    {
        return await courseManagementService.DeleteBlock(request.Id, request.BlockId);
    }
}
