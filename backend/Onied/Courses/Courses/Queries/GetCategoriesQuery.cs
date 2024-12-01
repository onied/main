using MediatR;

namespace Courses.Queries;

public record GetCategoriesQuery() : IRequest<IResult>;
