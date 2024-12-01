using Courses.Dtos.EditCourse.Request;
using MediatR;

namespace Courses.Commands;

public record AddBlockCommand(int Id, int ModuleId, AddBlockRequest AddBlockRequest) : IRequest<IResult>;
