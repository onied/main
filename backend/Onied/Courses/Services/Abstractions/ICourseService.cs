﻿namespace Courses.Services.Abstractions;

public interface ICourseService
{
    public Task<IResult> GetCoursePreview(int id, Guid? userId);

    public Task<IResult> EnterFreeCourse(int id, Guid userId);

    public Task<IResult> GetCourseHierarchy(int id, Guid userId);

    public Task<IResult> GetSummaryBlock(int id, int blockId, Guid userId);

    public Task<IResult> GetVideoBlock(int id, int blockId, Guid userId);

    public Task<IResult> GetEditTaskBlock(int id, int blockId, Guid userId);

    public Task<IResult> GetTaskBlock(int id, int blockId, Guid userId);

    public Task<IResult> CreateCourse(string? userId);
}
