using AutoMapper;
using Courses.Dtos;
using Courses.Models;
using Courses.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[ApiController]
[Route("api/v1/[controller]/{id:int}")]
public class CoursesController : ControllerBase
{
    private readonly IBlockRepository _blockRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICheckTasksService _checkTasksService;
    private readonly ICourseRepository _courseRepository;
    private readonly ILogger<CoursesController> _logger;
    private readonly IMapper _mapper;

    public CoursesController(
        ILogger<CoursesController> logger,
        IMapper mapper,
        ICourseRepository courseRepository,
        IBlockRepository blockRepository,
        ICheckTasksService checkTasksService, ICategoryRepository categoryRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _courseRepository = courseRepository;
        _blockRepository = blockRepository;
        _checkTasksService = checkTasksService;
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    public async Task<ActionResult<PreviewDto>> GetCoursePreview(int id)
    {
        var course = await _courseRepository.GetCourseAsync(id);
        if (course == null)
            return NotFound();
        return _mapper.Map<PreviewDto>(course);
    }

    [HttpGet]
    [Route("hierarchy")]
    public async Task<ActionResult<CourseDto>> GetCourseHierarchy(int id)
    {
        var course = await _courseRepository.GetCourseWithBlocksAsync(id);
        if (course == null)
            return NotFound();
        return _mapper.Map<CourseDto>(course);
    }

    [HttpGet]
    [Route("summary/{blockId:int}")]
    public async Task<ActionResult<SummaryBlockDto>> GetSummaryBlock(int id, int blockId)
    {
        var summary = await _blockRepository.GetSummaryBlock(blockId);
        if (summary == null || summary.Module.CourseId != id)
            return NotFound();
        return _mapper.Map<SummaryBlockDto>(summary);
    }

    [HttpGet]
    [Route("video/{blockId:int}")]
    public async Task<ActionResult<VideoBlockDto>> GetVideoBlock(int id, int blockId)
    {
        var block = await _blockRepository.GetVideoBlock(blockId);
        if (block == null || block.Module.CourseId != id)
            return NotFound();
        return _mapper.Map<VideoBlockDto>(block);
    }

    [HttpGet]
    [Route("tasks/{blockId:int}")]
    public async Task<ActionResult<TasksBlockDto>> GetTaskBlock(int id, int blockId)
    {
        var block = await _blockRepository.GetTasksBlock(blockId, true);
        if (block == null || block.Module.CourseId != id)
            return NotFound();
        return _mapper.Map<TasksBlockDto>(block);
    }

    [HttpGet]
    [Route("tasks/{blockId:int}/points")]
    public async Task<ActionResult<List<UserTaskPointsDto>>> GetTaskPointsStored(int id, int blockId)
    {
        var block = await _blockRepository.GetTasksBlock(blockId, true);

        if (block == null || block.Module.CourseId != id)
            return NotFound();

        var points = block.Tasks.Select(
            task => task.TaskType is TaskType.ManualReview
                ? null
                : new UserTaskPoints
                {
                    TaskId = task.Id,
                    Points = 0
                });
        // Заменить на нормальное обращение к базе
        // с нулевыми баллами для автомат. пров. заданий по умолчанию

        return _mapper.Map<List<UserTaskPointsDto>>(points);
    }

    [HttpPost]
    [Route("tasks/{blockId:int}/check")]
    public async Task<ActionResult<List<UserTaskPointsDto>>> CheckTaskBlock(
        int id,
        int blockId,
        [FromBody] List<UserInputDto> inputsDto)
    {
        if (inputsDto.Any(inputDto => inputDto is null))
            return BadRequest();

        var block = await _blockRepository.GetTasksBlock(
            blockId,
            true,
            true);

        if (block is null || block.Module.CourseId != id)
            return NotFound();

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


    [HttpPut]
    public async Task<Results<Ok<PreviewDto>, NotFound, ValidationProblem, UnauthorizedHttpResult>> EditCourse(int id,
        [FromQuery] string? userId,
        [FromBody] EditCourseDto editCourseDto)
    {
        var course = await _courseRepository.GetCourseAsync(id);
        if (course == null)
            return TypedResults.NotFound();
        if (userId == null || course.Author?.Id.ToString() != userId)
            return TypedResults.Unauthorized();
        var category = await _categoryRepository.GetCategoryById(editCourseDto.CategoryId);
        if (category == null)
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                [nameof(editCourseDto.CategoryId)] = ["This category does not exist."]
            });
        _mapper.Map(editCourseDto, course);
        course.Category = category;
        course.CategoryId = category.Id;
        await _courseRepository.UpdateCourseAsync(course);
        return TypedResults.Ok(_mapper.Map<PreviewDto>(course));
    }

    [HttpPut]
    [Route("edit/hierarchy")]
    public async Task<Results<Ok, NotFound, ForbidHttpResult>> EditHierarchy(
        int id,
        [FromQuery] string? userId,
        [FromBody] CourseDto courseDto)
    {
        var course = await _courseRepository.GetCourseWithBlocksAsync(id);
        if (course == null)
            return TypedResults.NotFound();
        if (userId == null || course.Author?.Id.ToString() != userId)
            return TypedResults.Forbid();
        _mapper.Map(courseDto, course);

        return TypedResults.Ok();
    }

    [HttpPut]
    [Route("video/{blockId:int}/edit")]
    public async Task<Results<Ok, NotFound, ForbidHttpResult>> EditVideoBlock(
        int id,
        int blockId,
        [FromQuery] string? userId,
        [FromBody] VideoBlockDto videoBlockDto)
    {
        var course = await _courseRepository.GetCourseAsync(id);
        if (course == null)
            return TypedResults.NotFound();
        if (userId == null || course.Author?.Id.ToString() != userId)
            return TypedResults.Forbid();

        var block = await _blockRepository.GetVideoBlock(blockId);
        if (block == null || block.Module.CourseId != id)
            return TypedResults.NotFound();

        _mapper.Map(videoBlockDto, block);
        await _blockRepository.UpdateVideoBlock(block);
        return TypedResults.Ok();
    }

    [HttpPut]
    [Route("summary/{blockId:int}/edit")]
    public async Task<Results<Ok, NotFound, ForbidHttpResult>> EditSummaryBlock(
        int id,
        int blockId,
        [FromQuery] string? userId,
        [FromBody] SummaryBlockDto summaryBlockDto)
    {
        var course = await _courseRepository.GetCourseAsync(id);
        if (course == null)
            return TypedResults.NotFound();
        if (userId == null || course.Author?.Id.ToString() != userId)
            return TypedResults.Forbid();

        var block = await _blockRepository.GetSummaryBlock(blockId);
        if (block == null || block.Module.CourseId != id)
            return TypedResults.NotFound();

        _mapper.Map(summaryBlockDto, block);
        await _blockRepository.UpdateSummaryBlock(block);
        return TypedResults.Ok();
    }

    [HttpPut]
    [Route("tasks/{blockId:int}/edit")]
    public async Task<Results<Ok, NotFound, ForbidHttpResult>> EditTasksBlock(
        int id,
        int blockId,
        [FromQuery] string? userId,
        [FromBody] TasksBlockDto tasksBlockDto)
    {
        var course = await _courseRepository.GetCourseAsync(id);
        if (course == null)
            return TypedResults.NotFound();
        if (userId == null || course.Author?.Id.ToString() != userId)
            return TypedResults.Forbid();

        var block = await _blockRepository.GetTasksBlock(blockId);
        if (block == null || block.Module.CourseId != id)
            return TypedResults.NotFound();

        _mapper.Map(tasksBlockDto, block);
        await _blockRepository.UpdateTasksBlock(block);
        return TypedResults.Ok();
    }

    [HttpGet]
    [Route("CheckEditCourse")]
    public async Task<Results<Ok, NotFound, ForbidHttpResult>> CheckEditCourse(
        int id,
        [FromQuery] string? userId)
    {
        var course = await _courseRepository.GetCourseAsync(id);
        if (course == null)
            return TypedResults.NotFound();
        if (userId == null || course.Author?.Id.ToString() != userId)
            return TypedResults.Forbid();
        return TypedResults.Ok();
    }
}
