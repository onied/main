using Courses.Dtos.EditCourse.Request;
using MediatR;

namespace Courses.Commands;

public record RenameModuleCommand(int Id, RenameModuleRequest RenameModuleRequest) : IRequest<IResult>;
