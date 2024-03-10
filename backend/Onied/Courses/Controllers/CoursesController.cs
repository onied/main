using AutoMapper;
using Courses.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Courses.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<CoursesController> _logger;
    private readonly IMapper _mapper;

    public CoursesController(ILogger<CoursesController> logger, IMapper mapper, AppDbContext context)
    {
        _logger = logger;
        _mapper = mapper;
        _context = context;
    }

    [HttpGet]
    public string Get()
    {
        return "Under construction...";
    }

    [HttpGet]
    [Route("{id:int}/get_preview")]
    public async Task<ActionResult<PreviewDto>> GetCoursePreview(int id)
    {
        var course = await _context.Courses
            .Include(course1 => course1.Modules)
            .Include(course1 => course1.Author)
            .Include(course1 => course1.Category)
            .FirstOrDefaultAsync(course1 => course1.Id == id);
        if (course == null)
            return NotFound();
        var preview = _mapper.Map<PreviewDto>(course);
        preview.CourseAuthor.Name = $"{course.Author.FirstName} {course.Author.LastName}";
        preview.CourseProgram = course.IsProgramVisible
            ? course.Modules.OrderBy(module => module.Id).Select(module => module.Title).ToList()
            : null;
        return preview;
    }

    [HttpGet]
    [Route("{id:int}/get_hierarchy")]
    public async Task<ActionResult<CourseDto>> GetCourseHierarchy(int id)
    {
        var course = await _context.Courses.Include(course1 => course1.Modules).ThenInclude(module => module.Blocks)
            .FirstOrDefaultAsync(course1 => course1.Id == id);
        if (course == null)
            return NotFound();
        return _mapper.Map<CourseDto>(course);
    }

    [HttpGet]
    [Route("{id:int}/get_summary_block/{blockId:int}")]
    public async Task<ActionResult<SummaryBlockDto>> GetSummaryBlock(int id, int blockId)
    {
        var summary = await _context.SummaryBlocks.Include(block => block.Module)
            .FirstOrDefaultAsync(block => block.Id == blockId);
        if (summary == null || summary.Module.CourseId != id)
            return NotFound();
        return _mapper.Map<SummaryBlockDto>(summary);
    }

    [HttpGet]
    [Route("{id:int}/get_video_block/{blockId:int}")]
    public async Task<ActionResult<VideoBlockDto>> GetVideoBlock(int id, int blockId)
    {
        var block = await _context.VideoBlocks.Include(block => block.Module)
            .FirstOrDefaultAsync(block => block.Id == blockId);
        if (block == null || block.Module.CourseId != id)
            return NotFound();
        return _mapper.Map<VideoBlockDto>(block);
    }
}