using MediatR;

namespace Courses.Commands;

public record DeleteModuleCommand(int Id, int ModuleId) : IRequest<IResult>;
