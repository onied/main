using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[ApiController]
[Route("api/v1/[controller]/{id:int}")]
public class CoursesController(ICourseService courseService) : ControllerBase
{
    [HttpGet]
    public async Task<IResult> GetCoursePreview(int id, [FromQuery] Guid? userId)
    {
        return await courseService.GetCoursePreview(id, userId);
    }

    [HttpPost("enter")]
    public async Task<IResult> EnterFreeCourse(int id, [FromQuery] Guid userId)
    {
        return await courseService.EnterFreeCourse(id, userId);
    }

    [HttpGet]
    [Route("hierarchy")]
    public async Task<IResult> GetCourseHierarchy(int id, [FromQuery] Guid userId,
        [FromQuery] string? role)
    {
        return await courseService.GetCourseHierarchy(id, userId, role);
    }

    [HttpGet]
    [Route("summary/{blockId:int}")]
    public async Task<IResult> GetSummaryBlock(int id, int blockId, [FromQuery] Guid userId,
        [FromQuery] string? role)
    {
        return await courseService.GetSummaryBlock(id, blockId, userId, role);
    }

    [HttpGet]
    [Route("video/{blockId:int}")]
    public async Task<IResult> GetVideoBlock(int id, int blockId, [FromQuery] Guid userId,
        [FromQuery] string? role)
    {
        return await courseService.GetVideoBlock(id, blockId, userId, role);
    }

    [HttpGet]
    [Route("tasks/{blockId:int}/for-edit")]
    public async Task<IResult> GetEditTaskBlock(int id, int blockId, [FromQuery] Guid userId,
        [FromQuery] string? role)
    {
        return await courseService.GetEditTaskBlock(id, blockId, userId, role);
    }

    [HttpGet]
    [Route("tasks/{blockId:int}")]
    public async Task<IResult> GetTaskBlock(int id, int blockId, [FromQuery] Guid userId,
        [FromQuery] string? role)
    {
        return await courseService.GetTaskBlock(id, blockId, userId, role);
    }

    [HttpPost]
    [Route("/api/v1/[controller]/create")]
    public async Task<IResult> CreateCourse(
        [FromQuery] string? userId)
    {
        return await courseService.CreateCourse(userId);
    }
}
