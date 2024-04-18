using System.Net;
using System.Text;
using System.Text.Json;
using Courses.Dtos;
using AutoMapper;
using Courses.Dtos.ModeratorDtos.Response;
using Courses.Enums;
using Courses.Extensions;
using Courses.Models;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Courses.Services;

public class CourseManagementService(
    ICourseRepository courseRepository,
    IUserCourseInfoRepository userCourseInfoRepository,
    IHttpClientFactory httpClientFactory,
    IMapper mapper) : ICourseManagementService
{
    public HttpClient PurchasesServerApiClient
        => httpClientFactory.CreateClient(ServerApiConfig.PurchasesServer.GetStringValue()!);

    public async Task<Results<Ok<Course>, NotFound, ForbidHttpResult>> CheckCourseAuthorAsync(int courseId, string? userId)
    {
        var course = await courseRepository.GetCourseAsync(courseId);
        if (course == null)
            return TypedResults.NotFound();
        if (userId == null || course.Author?.Id.ToString() != userId)
            return TypedResults.Forbid();
        return TypedResults.Ok(course);
    }

    public async Task<bool> AllowVisitCourse(Guid userId, int courseId)
    {
        var userCourseInfo = await userCourseInfoRepository.GetUserCourseInfoAsync(userId, courseId, true);
        if (userCourseInfo is null) return false;
        if (userCourseInfo.Course.PriceRubles == 0) return true;

        var requestString = JsonSerializer.Serialize(new VerifyTokenRequestDto(userCourseInfo.Token!));
        var response =
            await PurchasesServerApiClient.PostAsync(
                string.Empty,
                new StringContent(requestString, Encoding.UTF8, "application/json"));

        return response.StatusCode is HttpStatusCode.OK;
    }

    public async Task<Results<Ok<CourseStudentsDto>, NotFound, ForbidHttpResult>> GetStudents(int courseId, Guid authorId)
    {
        var course = await courseRepository.GetCourseWithUsersAndModeratorsAsync(courseId);
        if (course == null)
            return TypedResults.NotFound();
        if (course.Author?.Id != authorId)
            return TypedResults.Forbid();

        var courseDto = mapper.Map<CourseStudentsDto>(course);
        courseDto.Students.ForEach(s => s.IsModerator = course.Moderators.Any(x => x.Id == s.StudentId));

        return TypedResults.Ok(courseDto);
    }

    public async Task<Results<Ok, NotFound<string>, ForbidHttpResult>> DeleteModerator(int courseId, Guid studentId, Guid authorId)
    {
        var course = await courseRepository.GetCourseWithUsersAndModeratorsAsync(courseId);
        if (course == null)
            return TypedResults.NotFound<string>("Курс не найден");
        if (course.Author?.Id != authorId)
            return TypedResults.Forbid();
        if (course.Moderators.All(m => m.Id != studentId))
            return TypedResults.NotFound<string>("Удаляемый модератор не найден");

        await courseRepository.DeleteModeratorAsync(courseId, studentId);
        return TypedResults.Ok();
    }

    public async Task<Results<Ok, NotFound<string>, ForbidHttpResult>> AddModerator(int courseId, Guid studentId, Guid authorId)
    {
        var course = await courseRepository.GetCourseWithUsersAndModeratorsAsync(courseId);
        if (course == null)
            return TypedResults.NotFound<string>("Курс не найден");
        if (course.Author?.Id != authorId)
            return TypedResults.Forbid();
        if (course.Users.All(m => m.Id != studentId))
            return TypedResults.NotFound<string>("Добавляемый в модераторы ученик не найден");

        await courseRepository.AddModeratorAsync(courseId, studentId);
        return TypedResults.Ok();
    }
}
