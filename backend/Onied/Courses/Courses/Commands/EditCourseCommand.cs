using Courses.Dtos.EditCourse.Request;
using MediatR;

namespace Courses.Commands;

public record EditCourseCommand(int Id, EditCourseRequest EditCourseRequest, string? UserId) :
    IRequest<IResult>;
