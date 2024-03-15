using AutoMapper;
using Courses.Dtos;
using Courses.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Courses.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CatalogController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<CatalogController> _logger;
    private readonly IMapper _mapper;

    public CatalogController(ILogger<CatalogController> logger, IMapper mapper, AppDbContext context)
    {
        _logger = logger;
        _mapper = mapper;
        _context = context;
    }

    [HttpGet]
    public async Task<Page<CourseCardDto>> Get([FromQuery] PageQuery pageQuery)
    {
        var courses = _context.Courses.Include(course => course.Author).Include(course => course.Category);
        var page = Page<CourseCardDto>.Prepare(pageQuery, await courses.CountAsync(), out var offset);
        page.Elements = _mapper.Map<IEnumerable<CourseCardDto>>(courses.Skip(offset).Take(page.ElementsPerPage));
        return page;
    }
}
