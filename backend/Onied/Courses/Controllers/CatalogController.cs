using AutoMapper;
using Courses.Dtos;
using Courses.Helpers;
using Courses.Services;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CatalogController : ControllerBase
{
    private readonly ICourseRepository _courseRepository;
    private readonly ILogger<CatalogController> _logger;
    private readonly IMapper _mapper;

    public CatalogController(ILogger<CatalogController> logger, IMapper mapper, ICourseRepository courseRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _courseRepository = courseRepository;
    }

    [HttpGet]
    public async Task<Page<CourseCardDto>> Get([FromQuery] PageQuery pageQuery)
    {
        var page = Page<CourseCardDto>.Prepare(
            pageQuery,
            await _courseRepository.CountAsync(),
            out var offset);
        var courses = await _courseRepository.GetCoursesAsync(
            offset,
            page.ElementsPerPage);
        page.Elements = _mapper.Map<IEnumerable<CourseCardDto>>(courses);
        return page;
    }
}
