using AutoMapper;
using Courses.Dtos;
using Courses.Models;
using Courses.Services;
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
    public async Task<ActionResult<List<UserTaskPointsDto>>> GetTaskPointsStored(int id, int blockId)
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
        [FromBody] List<UserInputDto> inputsDto)
    {
        if (inputsDto.Any(inputDto => inputDto is null))
            return BadRequest();

        var block = await _context.TasksBlocks
            .Include(block => block.Module)
            .Include(block => block.Tasks)
                .ThenInclude(task => ((VariantsTask)task).Variants)
            .Include(block => block.Tasks)
                .ThenInclude(task => ((InputTask)task).Answers)
            .FirstOrDefaultAsync(block => block.Id == blockId);

        if (block is null || block.Module.CourseId != id)
            return NotFound();

        await System.Threading.Tasks.Task.Delay(2000);

        var points = new List<UserTaskPoints?>();
        foreach (var inputDto in inputsDto)
        {
            var task = block.Tasks.SingleOrDefault(task => inputDto.TaskId == task.Id);
            
            if (task is null)
                return NotFound($"Task with id={inputDto.TaskId} not found.");

            if (task.TaskType != inputDto.TaskType)
                return BadRequest($"Task with id={inputDto.TaskId} has invalid TaskType={inputDto.TaskType}.");
            
            points.Add(_checkTasksService.CheckTask(task, inputDto));
        }
        
        return _mapper.Map<List<UserTaskPointsDto>>(points);
    }
}