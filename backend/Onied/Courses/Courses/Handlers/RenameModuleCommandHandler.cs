using Courses.Commands;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class RenameModuleCommandHandler(ICourseManagementService courseManagementService)
    : IRequestHandler<RenameModuleCommand, IResult>
{
    public async Task<IResult> Handle(RenameModuleCommand request, CancellationToken cancellationToken)
    {
        return await courseManagementService.RenameModule(request.Id, request.RenameModuleRequest);
    }
}
