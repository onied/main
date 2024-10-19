using AutoFixture;
using Courses.Data;
using Courses.Data.Models;
using Courses.Services;
using Courses.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Tests.Courses.Helpers;
using Task = System.Threading.Tasks.Task;

namespace Tests.Courses.UnitTests.ServiceTests;

public class UserRepositoryTests
{
    private readonly Fixture _fixture = new();
    private readonly AppDbContext _context;
    private readonly IUserRepository _userRepository;

    private Guid _notExistingUserId;
    private Guid _existingUserId;

    public UserRepositoryTests()
    {
        _context = AppDbContextTest.GetContext();
        _userRepository = new UserRepository(_context);
        ProduceTestData();
    }

    [Fact]
    public async Task GetUsersWithConditionAsync_NoCondition_ReturnsAllUsers()
    {
        // Arrange
        var usersCount = await _context.Users.CountAsync();

        // Act
        var actual = await _userRepository.GetUsersWithConditionAsync();

        // Assert
        Assert.Equal(usersCount, actual.Count);
    }

    [Fact]
    public async Task GetUsersWithConditionAsync_WithCondition_ReturnsFilteredUsers()
    {
        // Arrange
        var userCondition = _fixture.Create<string>();
        var users = new List<User>
        {
            new User { Id = Guid.NewGuid(), FirstName = userCondition, LastName = userCondition},
            new User { Id = Guid.NewGuid(), FirstName = "OtherName", LastName = userCondition}
        };
        await _context.Users.AddRangeAsync(users);
        await _context.SaveChangesAsync();

        // Act
        var actual = await _userRepository.GetUsersWithConditionAsync(u => u.FirstName == userCondition);

        // Assert
        Assert.Single(actual);
        Assert.Equal(userCondition, actual.First().FirstName);
    }

    [Fact]
    public async Task GetUserAsync_ExistingUserId_ReturnsUser()
    {
        // Act
        var actual = await _userRepository.GetUserAsync(_existingUserId);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(_existingUserId, actual.Id);
    }

    [Fact]
    public async Task GetUserAsync_NonExistingUserId_ReturnsNull()
    {
        // Act
        var actual = await _userRepository.GetUserAsync(_notExistingUserId);

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public async Task GetUserWithCoursesAsync_ExistingUserId_ReturnsUserWithCourses()
    {
        // Act
        var actual = await _userRepository.GetUserWithCoursesAsync(_existingUserId);

        // Assert
        Assert.NotNull(actual);
        Assert.NotNull(actual.Courses);
    }

    [Fact]
    public async Task AddUserAsync_UserStored()
    {
        // Arrange
        var user = _fixture.Build<User>()
            .With(u => u.Id, Guid.NewGuid())
            .Create();

        // Act
        await _userRepository.AddUserAsync(user);
        var actualUser = await _context.Users.SingleOrDefaultAsync(u => u.Id == user.Id);

        // Assert
        Assert.NotNull(actualUser);
        Assert.Equal(user.FirstName, actualUser.FirstName);
    }

    [Fact]
    public async Task UpdateUserAsync_UserStored()
    {
        // Arrange
        var user = _fixture.Build<User>()
            .With(u => u.Id, Guid.NewGuid())
            .Create();
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        user.FirstName = "UpdatedName";

        // Act
        await _userRepository.UpdateUserAsync(user);
        var actualUser = await _context.Users.SingleOrDefaultAsync(u => u.Id == user.Id);

        // Assert
        Assert.NotNull(actualUser);
        Assert.Equal("UpdatedName", actualUser.FirstName);
    }

    private void ProduceTestData()
    {
        var user = _fixture.Build<User>()
            .With(u => u.Id, Guid.NewGuid())
            .Create();

        _existingUserId = user.Id;
        _notExistingUserId = Guid.NewGuid();

        _context.Users.Add(user);
        _context.SaveChanges();
    }
}
