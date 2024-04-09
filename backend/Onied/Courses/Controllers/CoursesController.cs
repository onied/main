using AutoMapper;
using Courses.Dtos;
using Courses.Services;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[ApiController]
[Route("api/v1/[controller]/{id:int}")]
public class CoursesController : ControllerBase
{
    private readonly IBlockRepository _blockRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IBlockCompletedInfoRepository _blockCompletedInfoRepository;
    private readonly IMapper _mapper;

    public CoursesController(
        ILogger<CoursesController> logger,
        IMapper mapper,
        ICourseRepository courseRepository,
        IBlockRepository blockRepository,
        IBlockCompletedInfoRepository blockCompletedInfoRepository)
    {
        _mapper = mapper;
        _courseRepository = courseRepository;
        _blockRepository = blockRepository;
        _blockCompletedInfoRepository = blockCompletedInfoRepository;
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
    public async Task<ActionResult<CourseDto>> GetCourseHierarchy(int id, [FromQuery] Guid userId)
    {
        var course = await _courseRepository.GetCourseWithBlocksAsync(id);
        if (course == null)
            return NotFound();

        var dto = _mapper.Map<CourseDto>(course);

        var completed =
            await _blockCompletedInfoRepository
                .GetAllCompletedCourseBlocksByUser(userId, id);
        var blocksLink = dto.Modules.Select(m => m.Blocks)
            .Aggregate((prev, next) => prev.Concat(next).ToList());
        foreach (var cm in completed)
            blocksLink.Single(b => b.Id == cm.BlockId).Completed = true;

        return dto;
    }

    [HttpGet]
    [Route("summary/{blockId:int}")]
    public async Task<ActionResult<SummaryBlockDto>> GetSummaryBlock(int id, int blockId, [FromQuery] Guid userId)
    {
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
    public async Task<ActionResult<EditTasksBlockDto>> GetEditTaskBlock(int id, int blockId)
    {
        var block = await _blockRepository.GetTasksBlock(blockId, true, true);
        if (block == null || block.Module.CourseId != id)
            return NotFound();
        return _mapper.Map<EditTasksBlockDto>(block);
    }

    [Route("tasks/{blockId:int}")]
    public async Task<ActionResult<TasksBlockDto>> GetTaskBlock(int id, int blockId, [FromQuery] Guid userId)
    {
        var block = await _blockRepository.GetTasksBlock(blockId, true);
        if (block == null || block.Module.CourseId != id)
            return NotFound();

        var dto = _mapper.Map<TasksBlockDto>(block);
        if (await _blockCompletedInfoRepository.GetCompletedCourseBlockAsync(userId, blockId) is not null)
            dto.IsCompleted = true;
        return dto;
    }
}
