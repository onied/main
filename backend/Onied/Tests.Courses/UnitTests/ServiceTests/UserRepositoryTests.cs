using AutoFixture;
using Courses;
using Courses.Models;
using Courses.Models.Users;
using Courses.Services;
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
    private IEnumerable<Course> _courses;

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
        Assert.Equal(_existingUserId, actual.Id);
    }

    [Fact]
    public async Task GetUserWithCourses_ReturnsUserWithCourses()
    {
        // Act
        var actual = await _userRepository.GetUserWithCoursesAsync(_existingUserId);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(_courses, actual.Courses);
    }

    [Fact]
    public async Task SaveUser_UserStored()
    {
        // Arrange
        var user = _fixture.Build<Author>()
            .With(author1 => author1.Id, Guid.NewGuid)
            .Create();

        // Act
        await _userRepository.SaveUserAsync(user);
        var actualUser = _context.Users.SingleOrDefault(u => u.Id == user.Id);

        // Assert
        Assert.NotNull(actualUser);
        Assert.Equal(user, actualUser);
    }

    private void ProduceTestData()
    {
        _notExistingUserId = Guid.NewGuid();

        var author = _fixture.Build<Author>()
            .With(author1 => author1.Id, Guid.NewGuid)
            .Create();

        _context.Authors.Add(author);

        _courses = Enumerable.Range(1, CourseSequenceLength)
                .Select(i => _fixture.Build<Course>()
                        .With(c => c.Id, i)
                        .With(c => c.AuthorId, author.Id)
                        .Create())
                .ToList();
        _context.Courses.AddRange(_courses);

        var user = _fixture.Build<User>()
            .With(u => u.Id, Guid.NewGuid)
            .Create();
        foreach (var c in _courses)
        {
            user.Courses.Add(c);
        }
        _context.Add(user);
        _existingUserId = user.Id;

        _context.SaveChanges();
    }
}
