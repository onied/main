using Courses.Dtos.Course.Response;
using MediatR;

namespace Courses.Commands;

public record EditHierarchyCommand(int Id, CourseResponse CourseResponse) : IRequest<IResult>;
