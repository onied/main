namespace Courses.Services.Abstractions;

public interface IAccountsService
{
    public Task<IResult> GetCourses(Guid id);
}
