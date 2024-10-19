using AutoFixture;
using AutoMapper;
using Courses.Data.Models;
using Courses.Dtos.ManualReview.Request;
using Courses.Dtos.ManualReview.Response;
using Courses.Services;
using Courses.Services.Abstractions;
using Courses.Services.Producers.NotificationSentProducer;
using MassTransit.Data.Messages;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace Tests.Courses.UnitTests.ServiceTests;

public class ManualReviewServiceTests
{
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IManualReviewTaskUserAnswerRepository> _manualReviewTaskUserAnswerRepository = new();
    private readonly Mock<ITaskCompletionService> _taskCompletionService = new();
    private readonly Mock<IUserTaskPointsRepository> _userTaskPointsRepository = new();
    private readonly Mock<INotificationSentProducer> _notificationSentProducer = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Fixture _fixture = new();

    private readonly ManualReviewService _service;

    public ManualReviewServiceTests()
    {
        _service = new ManualReviewService(
            _userRepository.Object,
            _manualReviewTaskUserAnswerRepository.Object,
            _taskCompletionService.Object,
            _userTaskPointsRepository.Object,
            _notificationSentProducer.Object,
            _mapper.Object
        );
    }

    [Fact]
    public async Task GetManualReviewTaskUserAnswer_UserNotFound_ReturnsUnauthorized()
    {
        // Arrange
        var teacherId = Guid.NewGuid();
        var answerId = Guid.NewGuid();

        // Act
        var result = await _service.GetManualReviewTaskUserAnswer(teacherId, answerId);

        // Assert
        Assert.IsType<UnauthorizedHttpResult>(result);
    }

    [Fact]
    public async Task GetManualReviewTaskUserAnswer_TaskNotFound_ReturnsNotFound()
    {
        // Arrange
        var teacherId = Guid.NewGuid();
        var answerId = Guid.NewGuid();
        var user = new User(); // Assume a valid user object

        _userRepository.Setup(repo => repo.GetUserWithModeratingAndTeachingCoursesAsync(teacherId))
            .ReturnsAsync(user);

        // Act
        var result = await _service.GetManualReviewTaskUserAnswer(teacherId, answerId);

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task ReviewUserAnswer_SuccessfulReview_ReturnsOk()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var answerId = Guid.NewGuid();
        var reviewRequest = new ReviewTaskRequest();
        var user = new User();
        var taskAnswer = _fixture.Create<ManualReviewTaskUserAnswer>();
        var pointsInfo = new List<UserTaskPoints>();

        _userRepository.Setup(repo => repo.GetUserWithModeratingAndTeachingCoursesAsync(userId))
            .ReturnsAsync(user);
        _manualReviewTaskUserAnswerRepository.Setup(repo => repo.GetById(answerId))
            .ReturnsAsync(taskAnswer);
        _userTaskPointsRepository.Setup(repo => repo.GetUserTaskPointsByUserAndBlock(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(pointsInfo);
        _manualReviewTaskUserAnswerRepository.Setup(repo =>
                repo.CanReviewAnswer(It.IsAny<User>(), It.IsAny<ManualReviewTaskUserAnswer>()))
            .Returns(true);

        // Act
        var result = await _service.ReviewUserAnswer(userId, answerId, reviewRequest);

        // Assert
        Assert.IsType<Ok>(result);
        _notificationSentProducer.Verify(x => x.PublishForOne(It.IsAny<NotificationSent>()), Times.Once);
        _taskCompletionService.Verify(x => x.ManageTaskBlockCompleted(pointsInfo, taskAnswer.UserId, taskAnswer.Task.TasksBlockId), Times.Once);
        _taskCompletionService.Verify(x => x.ManageCourseCompleted(taskAnswer.UserId, taskAnswer.CourseId), Times.Once);
    }

    [Fact]
    public async Task GetUncheckedForTeacher_Success_ReturnsOk()
    {
        // Arrange
        var teacherId = Guid.NewGuid();
        var user = new User();
        var answers = new List<ManualReviewTaskUserAnswer> { new ManualReviewTaskUserAnswer() };

        _userRepository.Setup(repo => repo.GetUserWithModeratingAndTeachingCoursesAsync(teacherId))
            .ReturnsAsync(user);
        _manualReviewTaskUserAnswerRepository.Setup(repo => repo.GetUncheckedTasksToReview(user))
            .ReturnsAsync(answers);
        _mapper.Setup(m => m.Map<List<ManualReviewTaskUserAnswerResponse>>(answers))
            .Returns(new List<ManualReviewTaskUserAnswerResponse>());

        // Act
        var result = await _service.GetUncheckedForTeacher(teacherId);

        // Assert
        Assert.IsType<Ok<List<ManualReviewTaskUserAnswerResponse>>>(result);
    }

    [Fact]
    public async Task GetCheckedForTeacher_UserNotFound_ReturnsUnauthorized()
    {
        // Arrange
        var teacherId = Guid.NewGuid();

        // Act
        var result = await _service.GetCheckedForTeacher(teacherId);

        // Assert
        Assert.IsType<UnauthorizedHttpResult>(result);
    }

    [Fact]
    public async Task GetCheckedForTeacher_Success_ReturnsOk()
    {
        // Arrange
        var teacherId = Guid.NewGuid();
        var user = new User(); // Simulate a valid user
        var checkedTasks = new List<ManualReviewTaskUserAnswer> { new ManualReviewTaskUserAnswer() };
        var mappedResponse = new List<ManualReviewTaskUserAnswerResponse>(); // Expected response after mapping

        _userRepository.Setup(repo => repo.GetUserWithModeratingAndTeachingCoursesAsync(teacherId))
            .ReturnsAsync(user);
        _manualReviewTaskUserAnswerRepository.Setup(repo => repo.GetCheckedTasksToReview(user))
            .ReturnsAsync(checkedTasks);
        _mapper.Setup(m => m.Map<List<ManualReviewTaskUserAnswerResponse>>(checkedTasks))
            .Returns(mappedResponse);

        // Act
        var result = await _service.GetCheckedForTeacher(teacherId);

        // Assert
        var okResult = Assert.IsType<Ok<List<ManualReviewTaskUserAnswerResponse>>>(result);
        Assert.Equal(mappedResponse, okResult.Value);
    }

    [Fact]
    public async Task GetTasksToCheckForTeacher_UserNotFound_ReturnsUnauthorized()
    {
        // Arrange
        var teacherId = Guid.NewGuid();

        // Act
        var result = await _service.GetTasksToCheckForTeacher(teacherId);

        // Assert
        Assert.IsType<UnauthorizedHttpResult>(result);
    }

    [Fact]
    public async Task GetTasksToCheckForTeacher_Success_ReturnsOk()
    {
        // Arrange
        var teacherId = Guid.NewGuid();
        var user = new User(); // Simulate a valid user
        var uncheckedTasks = new List<ManualReviewTaskUserAnswer> { new ManualReviewTaskUserAnswer() };
        var mappedResponse = new List<CourseWithManualReviewTasksResponse>(); // Expected response after mapping

        _userRepository.Setup(repo => repo.GetUserWithModeratingAndTeachingCoursesAsync(teacherId))
            .ReturnsAsync(user);
        _manualReviewTaskUserAnswerRepository.Setup(repo => repo.GetUncheckedTasksToReview(user))
            .ReturnsAsync(uncheckedTasks);
        _mapper.Setup(m => m.Map<List<CourseWithManualReviewTasksResponse>>(uncheckedTasks))
            .Returns(mappedResponse);

        // Act
        var result = await _service.GetTasksToCheckForTeacher(teacherId);

        // Assert
        var okResult = Assert.IsType<Ok<List<CourseWithManualReviewTasksResponse>>>(result);
        Assert.Equal(mappedResponse, okResult.Value);
    }
}
