using AutoMapper;
using Courses.Dtos;
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
        return Results.Ok(mapper.Map<List<CategoryDto>>(await categoryRepository.GetAllCategoriesAsync()));
    }
}
