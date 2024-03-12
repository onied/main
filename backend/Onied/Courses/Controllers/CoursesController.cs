using AutoMapper;
using Courses.Dtos;
using Courses.Dtos.Blocks.Tasks;
using Courses.Dtos.Blocks.Tasks.TaskUserInput;
using Courses.Models;
using Courses.Models.Blocks.Tasks;
using Courses.Models.Blocks.Tasks.TaskUserInput;
using Courses.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Courses.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<CoursesController> _logger;
    private readonly IMapper _mapper;
    private readonly ICheckTasksService _checkTasksService;

    public CoursesController(
        ILogger<CoursesController> logger,
        IMapper mapper,
        AppDbContext context,
        ICheckTasksService checkTasksService)
    {
        _logger = logger;
        _mapper = mapper;
        _context = context;
        _checkTasksService = checkTasksService;
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
        return _mapper.Map<PreviewDto>(course);
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

    [HttpGet]
    [Route("{id:int}/get_tasks_block/{blockId:int}")]
    public async Task<ActionResult<TasksBlockDto>> GetTaskBlock(int id, int blockId)
    {
        var block = await _context.TasksBlocks
            .Include(block => block.Module)
            .Include(block => block.Tasks)
            .ThenInclude(task => ((VariantsTask)task).Variants)
            .FirstOrDefaultAsync(block => block.Id == blockId);
        if (block == null || block.Module.CourseId != id)
            return NotFound();
        return _mapper.Map<TasksBlockDto>(block);
    }

    [HttpGet]
    [Route("{id:int}/get_tasks_points/{blockId:int}")]
    public async Task<ActionResult<List<UserTaskPointsDto>>> CheckTaskBlock(int id, int blockId)
    {
        var block = await _context.TasksBlocks
            .Include(block => block.Module)
            .Include(block => block.Tasks)
            .FirstOrDefaultAsync(block => block.Id == blockId);

        if (block == null || block.Module.CourseId != id)
            return NotFound();

        var points = block.Tasks.Select(
            task => task.TaskType is TaskType.ManualReview
                ? null
                : new UserTaskPoints()
                {
                    TaskId = task.Id,
                    Points = 0
                });
        // Заменить на нормальное обращение к базе
        // с нулевыми баллами для автомат. пров. заданий по умолчанию

        return _mapper.Map<List<UserTaskPointsDto>>(points);
    }

    [HttpPost]
    [Route("{id:int}/check_tasks_block/{blockId:int}")]
    public async Task<ActionResult<List<UserTaskPointsDto>>> CheckTaskBlock(
        int id,
        int blockId,
        [FromBody] IEnumerable<UserInputDto> inputsDto)
    {
        if (inputsDto.Any(inputDto => inputDto is null))
            return BadRequest();
        
        var block = await _context.TasksBlocks
            .Include(block => block.Module)
            .Include(block => block.Tasks)
            .ThenInclude(task => ((VariantsTask)task).Variants)
            .FirstOrDefaultAsync(block => block.Id == blockId);

        if (block is null || block.Module.CourseId != id)
            return NotFound();

        var inputs = _mapper.Map<List<UserInput>>(inputsDto);
        var pointsAsyncEnumerable = inputs.Select(async input => await _checkTasksService.CheckTask(input));
        return _mapper.Map<List<UserTaskPointsDto>>(await Task.WhenAll(pointsAsyncEnumerable));
    }
}