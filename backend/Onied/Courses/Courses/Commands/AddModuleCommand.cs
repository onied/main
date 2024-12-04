using MediatR;

namespace Courses.Commands;

public record AddModuleCommand(int Id) : IRequest<IResult>;
