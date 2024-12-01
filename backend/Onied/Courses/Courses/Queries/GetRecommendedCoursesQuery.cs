using Courses.Dtos.Catalog.Response;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Courses.Queries;

public record GetRecommendedCoursesQuery(int CoursesAmount)
    : IRequest<Results<Ok<List<CourseCardResponse>>, NotFound>>;
