using AutoMapper;
using Courses.Dtos;
using Courses.Models;
using Courses.Services.Abstractions;
using Courses.Services.Producers.CourseCreatedProducer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[ApiController]
[Route("api/v1/[controller]/{id:int}")]
public class CoursesController : ControllerBase
{
    private readonly IBlockRepository _blockRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBlockCompletedInfoRepository _blockCompletedInfoRepository;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IUserCourseInfoRepository _userCourseInfoRepository;
    private readonly ICourseCreatedProducer _courseCreatedProducer;
    private readonly ICourseManagementService _courseManagementService;

    public CoursesController(
        ILogger<CoursesController> logger,
        IMapper mapper,
        ICourseRepository courseRepository,
        IBlockRepository blockRepository,
        ICategoryRepository categoryRepository,
        IUserRepository userRepository,
        IUserCourseInfoRepository userCourseInfoRepository,
        IBlockCompletedInfoRepository blockCompletedInfoRepository,
        ICourseCreatedProducer courseCreatedProducer,
        ICourseManagementService courseManagementService)
    {
        _mapper = mapper;
        _courseRepository = courseRepository;
        _blockRepository = blockRepository;
        _userRepository = userRepository;
        _blockCompletedInfoRepository = blockCompletedInfoRepository;
        _categoryRepository = categoryRepository;
        _courseCreatedProducer = courseCreatedProducer;
        _courseManagementService = courseManagementService;
        _userCourseInfoRepository = userCourseInfoRepository;
    }

    [HttpGet]
    public async Task<ActionResult<PreviewDto>> GetCoursePreview(int id, [FromQuery] Guid? userId)
    {
        var course = await _courseRepository.GetCourseAsync(id);
        if (course == null)
            return NotFound();

        var userCourseInfo = userId is null
            ? null
            : await _userCourseInfoRepository.GetUserCourseInfoAsync(userId.Value, id);

        var preview = _mapper.Map<PreviewDto>(course);
        preview.IsOwned = userCourseInfo is not null;
        return preview;
    }

    [HttpPost("enter")]
    public async Task<ActionResult> EnterFreeCourse(int id, [FromQuery] Guid userId)
    {
        var course = await _courseRepository.GetCourseAsync(id);
        if (course == null || course.PriceRubles > 0) return NotFound();

        var maybeAlreadyEntered = await _userCourseInfoRepository
            .GetUserCourseInfoAsync(userId, course.Id);
        if (maybeAlreadyEntered is not null) return Forbid();
        var userCourseInfo = new UserCourseInfo()
        {
            UserId = userId,
            CourseId = course.Id
        };
        await _userCourseInfoRepository.AddUserCourseInfoAsync(userCourseInfo);
        return Ok();
    }

    [HttpGet]
    [Route("hierarchy")]
    public async Task<ActionResult<CourseDto>> GetCourseHierarchy(int id, [FromQuery] Guid userId)
    {
        var course = await _courseRepository.GetCourseWithBlocksAsync(id);
        if (course == null) return NotFound();

        if (!await _courseManagementService.AllowVisitCourse(userId, id)) return Forbid();

        var dto = _mapper.Map<CourseDto>(course);

        var completed =
            await _blockCompletedInfoRepository
                .GetAllCompletedCourseBlocksByUser(userId, id);
        var blocksLink = dto.Modules.SelectMany(m => m.Blocks).ToList();
        foreach (var cm in completed)
            blocksLink.Single(b => b.Id == cm.BlockId).Completed = true;

        return dto;
    }

    [HttpGet]
    [Route("summary/{blockId:int}")]
    public async Task<ActionResult<SummaryBlockDto>> GetSummaryBlock(int id, int blockId, [FromQuery] Guid userId)
    {
        if (!await _courseManagementService.AllowVisitCourse(userId, id)) return Forbid();

        var summary = await _blockRepository.GetSummaryBlock(blockId);
        if (summary == null || summary.Module.CourseId != id)
            return NotFound();

        var dto = _mapper.Map<SummaryBlockDto>(summary);
        if (await _blockCompletedInfoRepository.GetCompletedCourseBlockAsync(userId, blockId) is null)
            await _blockCompletedInfoRepository.AddCompletedCourseBlockAsync(userId, blockId);
        dto.IsCompleted = true;
        return dto;
    }

    [HttpGet]
    [Route("video/{blockId:int}")]
    public async Task<ActionResult<VideoBlockDto>> GetVideoBlock(int id, int blockId, [FromQuery] Guid userId)
    {
        if (!await _courseManagementService.AllowVisitCourse(userId, id)) return Forbid();

        var block = await _blockRepository.GetVideoBlock(blockId);
        if (block == null || block.Module.CourseId != id)
            return NotFound();

        var dto = _mapper.Map<VideoBlockDto>(block);
        if (await _blockCompletedInfoRepository.GetCompletedCourseBlockAsync(userId, blockId) is null)
            await _blockCompletedInfoRepository.AddCompletedCourseBlockAsync(userId, blockId);
        dto.IsCompleted = true;
        return dto;
    }

    [HttpGet]
    [Route("tasks/{blockId:int}/for-edit")]
    public async Task<ActionResult<EditTasksBlockDto>> GetEditTaskBlock(int id, int blockId, [FromQuery] Guid userId)
    {
        if (!await _courseManagementService.AllowVisitCourse(userId, id)) return Forbid();

        var block = await _blockRepository.GetTasksBlock(blockId, true, true);
        if (block == null || block.Module.CourseId != id)
            return NotFound();
        return _mapper.Map<EditTasksBlockDto>(block);
    }

    [HttpGet]
    [Route("tasks/{blockId:int}")]
    public async Task<ActionResult<TasksBlockDto>> GetTaskBlock(int id, int blockId, [FromQuery] Guid userId)
    {
        if (!await _courseManagementService.AllowVisitCourse(userId, id)) return Forbid();

        var block = await _blockRepository.GetTasksBlock(blockId, true);
        if (block == null || block.Module.CourseId != id)
            return NotFound();

        var dto = _mapper.Map<TasksBlockDto>(block);
        if (await _blockCompletedInfoRepository.GetCompletedCourseBlockAsync(userId, blockId) is not null)
            dto.IsCompleted = true;
        return dto;
    }

    [HttpPost]
    [Route("/api/v1/[controller]/create")]
    public async Task<Results<Ok<CreateCourseResponseDto>, UnauthorizedHttpResult>> CreateCourse(
        [FromQuery] string? userId)
    {
        if (userId == null || !Guid.TryParse(userId, out var authorId))
            return TypedResults.Unauthorized();
        var user = await _userRepository.GetUserAsync(authorId);
        if (user == null)
            return TypedResults.Unauthorized();
        var newCourse = await _courseRepository.AddCourseAsync(new Course
        {
            AuthorId = user.Id,
            Title = "Без названия",
            Description = "Без описания",
            PictureHref = "https://upload.wikimedia.org/wikipedia/commons/3/3f/Placeholder_view_vector.svg",
            CategoryId = (await _categoryRepository.GetAllCategoriesAsync())[0].Id
        });
        await _courseCreatedProducer.PublishAsync(newCourse);
        return TypedResults.Ok(new CreateCourseResponseDto
        {
            Id = newCourse.Id
        });
    }
}
