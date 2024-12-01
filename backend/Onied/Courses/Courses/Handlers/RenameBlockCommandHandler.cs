using Courses.Commands;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class RenameBlockCommandHandler(ICourseManagementService courseManagementService) : IRequestHandler<RenameBlockCommand, IResult>
{
    public async Task<IResult> Handle(RenameBlockCommand request, CancellationToken cancellationToken)
    {
        return await courseManagementService.RenameBlock(request.Id, request.RenameBlockRequest);
    }
}
