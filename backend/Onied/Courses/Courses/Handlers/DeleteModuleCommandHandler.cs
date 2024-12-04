using Courses.Commands;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class DeleteModuleCommandHandler(ICourseManagementService courseManagementService) : IRequestHandler<DeleteModuleCommand, IResult>
{
    public async Task<IResult> Handle(DeleteModuleCommand request, CancellationToken cancellationToken)
    {
        return await courseManagementService.DeleteModule(request.Id, request.ModuleId);
    }
}
