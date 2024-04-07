using AutoMapper;
using Courses.Dtos;
using Courses.Dtos.TaskCheckDtos;
using Courses.Dtos.TaskCheckDtos.Request;
using Courses.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[ApiController]
[Route("api/v1/[controller]/{userId:guid}")]
public class TeachingController(
    IUserRepository userRepository,
    ITaskCheckService taskCheckService,
    IMapper mapper) : ControllerBase
{
    [HttpGet]
    [Route("authored")]
    public async Task<Results<Ok<List<CourseCardDto>>, NotFound>> GetAuthoredCourses(Guid userId)
    {
        var user = await userRepository.GetUserWithTeachingCoursesAsync(userId);
        if (user is null) return TypedResults.NotFound();

        return TypedResults.Ok(mapper.Map<List<CourseCardDto>>(user.TeachingCourses));
    }

    [HttpGet]
    [Route("moderated")]
    public async Task<Results<Ok<List<CourseCardDto>>, NotFound>> GetModeratedCourses(Guid userId)
    {
        var user = await userRepository.GetUserWithModeratingCoursesAsync(userId);
        if (user is null) return TypedResults.NotFound();

        return TypedResults.Ok(mapper.Map<List<CourseCardDto>>(user.ModeratingCourses));
    }

    [HttpGet]
    [Route("check/{taskCheckId:guid}")]
    public async Task<Results<Ok<TaskCheckDto>, NotFound, UnauthorizedHttpResult, ForbidHttpResult>> GetTaskToCheck(
        Guid userId,
        Guid taskCheckId)
    {
        return await taskCheckService.GetTaskCheck(userId, taskCheckId);
    }

    [HttpPut]
    [Route("check/{taskCheckId:guid}")]
    public async Task<Results<Ok, NotFound, UnauthorizedHttpResult, ForbidHttpResult, ValidationProblem>> CheckTask(
        Guid userId,
        Guid taskCheckId, [FromBody] CheckTaskDto checkTaskDto)
    {
        return await taskCheckService.CheckTask(userId, taskCheckId, checkTaskDto);
    }
}
