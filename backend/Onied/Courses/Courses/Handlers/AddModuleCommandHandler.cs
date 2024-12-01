using Courses.Commands;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class AddModuleCommandHandler(ICourseManagementService courseManagementService) : IRequestHandler<AddModuleCommand, IResult>
{
    public async Task<IResult> Handle(AddModuleCommand request, CancellationToken cancellationToken)
    {
        return await courseManagementService.AddModule(request.Id);
    }
}
