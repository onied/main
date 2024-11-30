using Courses.Dtos.EditCourse.Request;
using MediatR;

namespace Courses.Commands;

public record RenameBlockCommand(int Id, RenameBlockRequest RenameBlockRequest) : IRequest<IResult>;
