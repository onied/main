namespace Courses.Services.Abstractions;

public interface ITeachingService
{
    public Task<IResult> GetAuthoredCourses(Guid userId);

    public Task<IResult> GetModeratedCourses(Guid userId);
}
