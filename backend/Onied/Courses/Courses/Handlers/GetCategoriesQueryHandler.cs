using AutoMapper;
using Courses.Dtos.Catalog.Response;
using Courses.Queries;
using Courses.Services.Abstractions;
using MediatR;

namespace Courses.Handlers;

public class GetCategoriesQueryHandler(IMapper mapper, ICategoryRepository categoryRepository)
    : IRequestHandler<GetCategoriesQuery, IResult>
{
    public async Task<IResult> Handle(
        GetCategoriesQuery request,
        CancellationToken cancellationToken
    )
    {
        return Results.Ok(
            mapper.Map<List<CategoryResponse>>(await categoryRepository.GetAllCategoriesAsync())
        );
    }
}
