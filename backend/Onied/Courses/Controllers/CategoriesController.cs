using AutoMapper;
using Courses.Dtos;
using Courses.Dtos.Catalog.Response;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CategoriesController(IMapper mapper, ICategoryRepository categoryRepository) : ControllerBase
{
    [HttpGet]
    public async Task<IResult> GetCategories()
    {
        return Results.Ok(mapper.Map<List<CategoryResponse>>(await categoryRepository.GetAllCategoriesAsync()));
    }
}
