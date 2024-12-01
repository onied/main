using Courses.Dtos.EditCourse.Request;
using MediatR;

namespace Courses.Commands;

public record EditTasksBlockCommand(int Id, int BlockId, EditTasksBlockRequest TasksBlockRequest) : IRequest<IResult>;
