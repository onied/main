using Courses.Dtos.Catalog.Response;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Courses.Queries;

public record GetMostPopularCoursesQuery(int CoursesAmount)
    : IRequest<Results<Ok<List<CourseCardResponse>>, NotFound>>;
