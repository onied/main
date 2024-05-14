using AutoMapper;
using Courses.Dtos;
using Courses.Dtos.ManualReviewDtos.Request;
using Courses.Dtos.ManualReviewDtos.Response;
using Courses.Services;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Controllers;

[ApiController]
[Route("api/v1/[controller]/{userId:guid}")]
public class TeachingController(
    IUserRepository userRepository,
    IManualReviewService manualReviewService,
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
    [Route("tasks-to-check-list")]
    public async Task<Results<Ok<List<CourseWithManualReviewTasksDto>>, UnauthorizedHttpResult>>
        GetTasksToCheckList(Guid userId)
    {
        return await manualReviewService.GetTasksToCheckForTeacher(userId);
    }

    [HttpGet]
    [Route("check/{userAnswerId:guid}")]
    public async Task<Results<Ok<ManualReviewTaskUserAnswerDto>, NotFound, UnauthorizedHttpResult, ForbidHttpResult>>
        GetTaskToCheck(
        Guid userId,
        Guid userAnswerId)
    {
        return await manualReviewService.GetManualReviewTaskUserAnswer(userId, userAnswerId);
    }

    [HttpPut]
    [Route("check/{userAnswerId:guid}")]
    public async Task<Results<Ok, NotFound, UnauthorizedHttpResult, ForbidHttpResult, ValidationProblem>> CheckTask(
        Guid userId,
        Guid userAnswerId, [FromBody] ReviewTaskDto reviewTaskDto)
    {
        return await manualReviewService.ReviewUserAnswer(userId, userAnswerId, reviewTaskDto);
    }
}
