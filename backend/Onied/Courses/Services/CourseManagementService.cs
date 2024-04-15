using AutoMapper;
using Courses.Dtos.ModeratorDtos.Response;
using Courses.Models;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Courses.Services;

public class CourseManagementService(
    ICourseRepository courseRepository,
    IMapper mapper) : ICourseManagementService
{
    public async Task<Results<Ok<Course>, NotFound, ForbidHttpResult>> CheckCourseAuthorAsync(int courseId, string? userId)
    {
        var course = await courseRepository.GetCourseAsync(courseId);
        if (course == null)
            return TypedResults.NotFound();
        if (userId == null || course.Author?.Id.ToString() != userId)
            return TypedResults.Forbid();
        return TypedResults.Ok(course);
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
