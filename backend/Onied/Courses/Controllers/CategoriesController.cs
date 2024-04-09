using AutoMapper;
using Courses.Dtos;
using Courses.Services;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CategoriesController(IMapper mapper, ICategoryRepository categoryRepository) : ControllerBase
{
    [HttpGet]
    public async Task<Ok<List<CategoryDto>>> GetCategories()
    {
        return TypedResults.Ok(mapper.Map<List<CategoryDto>>(await categoryRepository.GetAllCategoriesAsync()));
    }
}
