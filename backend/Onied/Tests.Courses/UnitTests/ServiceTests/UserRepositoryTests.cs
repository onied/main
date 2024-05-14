using AutoFixture;
using Courses.Data;
using Courses.Data.Models;
using Courses.Services;
using Courses.Services.Abstractions;
using Task = System.Threading.Tasks.Task;

namespace Tests.Courses.UnitTests.ServiceTests;

public class UserRepositoryTests
{
    private readonly Fixture _fixture = new();

    private readonly AppDbContext _context;
    private readonly IUserRepository _userRepository;

    private const int CourseSequenceLength = 2;

    private Guid _notExistingUserId;
    private Guid _existingUserId;
    private IEnumerable<Course> _courses = null!;

    public UserRepositoryTests()
    {
        _context = ContextGenerator.GetContext();
        _userRepository = new UserRepository(_context);
        ProduceTestData();
    }

    [Fact]
    public async Task GetUser_ReturnsNull()
    {
        // Act
        var actual = await _userRepository.GetUserAsync(_notExistingUserId);

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public async Task GetUser_ReturnsUser()
    {
        // Act
        var actual = await _userRepository.GetUserAsync(_existingUserId);

        // Assert
        Assert.NotNull(actual);
        Assert.Equivalent(_existingUserId, actual.Id);
    }

    [Fact]
    public async Task GetUserWithCourses_ReturnsUserWithCourses()
    {
        // Act
        var actual = await _userRepository.GetUserWithCoursesAsync(_existingUserId);

        // Assert
        Assert.NotNull(actual);
        Assert.Equivalent(_courses, actual.Courses);
    }

    [Fact]
    public async Task AddUser_UserStored()
    {
        // Arrange
        var user = _fixture.Build<User>()
            .With(author1 => author1.Id, Guid.NewGuid)
            .Create();

        // Act
        await _userRepository.AddUserAsync(user);
        var actualUser = _context.Users.SingleOrDefault(u => u.Id == user.Id);

        // Assert
        Assert.NotNull(actualUser);
        Assert.Equivalent(user, actualUser);
    }

    [Fact]
    public async Task UpdateUser_UserStored()
    {
        // Arrange
        var user = _fixture.Build<User>()
            .With(author1 => author1.Id, Guid.NewGuid)
            .Create();
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var prevFirstName = user.FirstName;
        user.FirstName = "newName";

        // Act
        await _userRepository.UpdateUserAsync(user);
        var actualUser = _context.Users.SingleOrDefault(u => u.Id == user.Id);

        // Assert
        Assert.NotNull(actualUser);
        Assert.Equivalent(user, actualUser);
    }

    private void ProduceTestData()
    {
        var author = _fixture.Build<User>()
            .With(author1 => author1.Id, Guid.NewGuid)
            .Create();

        var courses = Enumerable.Range(1, CourseSequenceLength)
                .Select(i => _fixture.Build<Course>()
                        .With(c => c.Id, i)
                        .With(c => c.AuthorId, author.Id)
                        .Create())
                .ToList();

        var user = _fixture.Build<User>()
            .With(u => u.Id, Guid.NewGuid)
            .Create();
        foreach (var c in courses)
        {
            user.Courses.Add(c);
        }

        _existingUserId = user.Id;
        _notExistingUserId = Guid.NewGuid();
        _courses = courses;

        _context.Courses.AddRange(courses);
        _context.Users.Add(author);
        _context.Users.Add(user);
        _context.SaveChanges();
    }
}
