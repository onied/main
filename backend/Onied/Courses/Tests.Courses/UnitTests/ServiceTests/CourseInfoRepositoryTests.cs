using Courses.Data;
using Courses.Data.Models;
using Courses.Services;
using Courses.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Tests.Courses.Helpers;
using Task = System.Threading.Tasks.Task;

namespace Tests.Courses.UnitTests.ServiceTests;

public class UserCourseInfoRepositoryTests
{
    private readonly AppDbContext _context;
    private readonly IUserCourseInfoRepository _userCourseInfoRepository;

    public UserCourseInfoRepositoryTests()
    {
        _context = AppDbContextTest.GetContext();
        _userCourseInfoRepository = new UserCourseInfoRepository(_context);
        ProduceTestData();
    }

    private void ProduceTestData()
    {
        var user = new User { Id = Guid.NewGuid(), FirstName = "Test", LastName = "User" };
        var course = new Course { Id = 1, Title = "Test Course", Description = "", PictureHref = "" };
        var userCourseInfo = new UserCourseInfo { UserId = user.Id, CourseId = course.Id };

        _context.Users.Add(user);
        _context.Courses.Add(course);
        _context.UserCourseInfos.Add(userCourseInfo);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetUserCourseInfoAsync_ExistingUserCourseInfo_ReturnsCorrectInfo()
    {
        // Arrange
        var user = await _context.Users.FirstAsync();
        var course = await _context.Courses.FirstAsync();

        // Act
        var actual = await _userCourseInfoRepository.GetUserCourseInfoAsync(user.Id, course.Id);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(user.Id, actual.UserId);
        Assert.Equal(course.Id, actual.CourseId);
    }

    [Fact]
    public async Task GetUserCourseInfoAsync_NonExistingUserCourseInfo_ReturnsNull()
    {
        // Arrange
        var nonExistingUserId = Guid.NewGuid();
        var course = await _context.Courses.FirstAsync();

        // Act
        var actual = await _userCourseInfoRepository.GetUserCourseInfoAsync(nonExistingUserId, course.Id);

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public async Task AddUserCourseInfoAsync_NewUserCourseInfo_StoredSuccessfully()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid(), FirstName = "New", LastName = "User" };
        var course = new Course { Id = 2, Title = "New Course", Description = "", PictureHref = "" };
        var userCourseInfo = new UserCourseInfo { UserId = user.Id, CourseId = course.Id };

        _context.Users.Add(user);
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        // Act
        var result = await _userCourseInfoRepository.AddUserCourseInfoAsync(userCourseInfo);

        // Assert
        Assert.True(result);
        var actual = await _context.UserCourseInfos.SingleOrDefaultAsync(uci => uci.UserId == user.Id && uci.CourseId == course.Id);
        Assert.NotNull(actual);
    }

    [Fact]
    public async Task AddUserCourseInfoAsync_ExistingUserCourseInfo_ReturnsFalse()
    {
        // Arrange
        var userCourseInfo = await _context.UserCourseInfos.FirstAsync();

        // Act
        var result = await _userCourseInfoRepository.AddUserCourseInfoAsync(userCourseInfo);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateUserCourseInfoAsync_ExistingUserCourseInfo_UpdatedSuccessfully()
    {
        // Arrange
        var userCourseInfo = await _context.UserCourseInfos.FirstAsync();
        userCourseInfo.Token = "new";

        // Act
        var result = await _userCourseInfoRepository.UpdateUserCourseInfoAsync(userCourseInfo);

        // Assert
        Assert.True(result);
        var updatedInfo = await _context.UserCourseInfos.FindAsync(userCourseInfo.UserId, userCourseInfo.CourseId);
        Assert.NotNull(updatedInfo);
        Assert.Equal("new", updatedInfo.Token);
    }

    [Fact]
    public async Task UpdateUserCourseInfoAsync_NonExistingUserCourseInfo_ReturnsFalse()
    {
        // Arrange
        var userCourseInfo = new UserCourseInfo { UserId = Guid.NewGuid(), CourseId = 999 }; // Non-existing

        // Act
        var result = await _userCourseInfoRepository.UpdateUserCourseInfoAsync(userCourseInfo);

        // Assert
        Assert.False(result);
    }
}
