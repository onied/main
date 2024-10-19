using Courses.Data;
using Courses.Data.Models;
using Courses.Services;
using Courses.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Tests.Courses.Helpers;
using Task = System.Threading.Tasks.Task;

namespace Tests.Courses.UnitTests.ServiceTests;

public class BlockCompletedInfoRepositoryTests
{
    private readonly AppDbContext _context;
    private readonly IBlockCompletedInfoRepository _blockCompletedInfoRepository;

    public BlockCompletedInfoRepositoryTests()
    {
        _context = AppDbContextTest.GetContext();
        _blockCompletedInfoRepository = new BlockCompletedInfoRepository(_context);
        ProduceTestData();
    }

    private void ProduceTestData()
    {
        var user = new User { Id = Guid.NewGuid(), FirstName = "Test", LastName = "User" };
        var course = new Course { Id = 1, Title = "Test Course", Description = "", PictureHref = "" };
        var module = new Module { Id = 1, CourseId = course.Id, Title = "Test Module" };
        var block = new TasksBlock { Id = 1, ModuleId = module.Id, Title = "Test Block" };
        var blockCompletedInfo = new BlockCompletedInfo { UserId = user.Id, BlockId = block.Id };

        _context.Users.Add(user);
        _context.Courses.Add(course);
        _context.Modules.Add(module);
        _context.TasksBlocks.Add(block);
        _context.BlockCompletedInfos.Add(blockCompletedInfo);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetAllCompletedCourseBlocksByUser_ReturnsCorrectBlocks()
    {
        // Arrange
        var user = await _context.Users.FirstAsync();
        var course = await _context.Courses.FirstAsync();

        // Act
        var actual = await _blockCompletedInfoRepository.GetAllCompletedCourseBlocksByUser(user.Id, course.Id);

        // Assert
        Assert.Single(actual);
        Assert.Equal(user.Id, actual[0].UserId);
    }

    [Fact]
    public async Task GetCompletedCourseBlockAsync_ExistingBlock_ReturnsBlockInfo()
    {
        // Arrange
        var user = await _context.Users.FirstAsync();
        var blockCompletedInfo = await _context.BlockCompletedInfos.FirstAsync();

        // Act
        var actual = await _blockCompletedInfoRepository.GetCompletedCourseBlockAsync(user.Id, blockCompletedInfo.BlockId);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(user.Id, actual.UserId);
        Assert.Equal(blockCompletedInfo.BlockId, actual.BlockId);
    }

    [Fact]
    public async Task GetCompletedCourseBlockAsync_NonExistingBlock_ReturnsNull()
    {
        // Arrange
        var nonExistingBlockId = 999;
        var user = await _context.Users.FirstAsync();

        // Act
        var actual = await _blockCompletedInfoRepository.GetCompletedCourseBlockAsync(user.Id, nonExistingBlockId);

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public async Task AddCompletedCourseBlockAsync_NewBlock_StoredSuccessfully()
    {
        // Arrange
        var user = await _context.Users.FirstAsync();
        var block = new TasksBlock { Id = 2, ModuleId = 1, Title = "New Block" };
        _context.TasksBlocks.Add(block);
        await _context.SaveChangesAsync();

        // Act
        await _blockCompletedInfoRepository.AddCompletedCourseBlockAsync(user.Id, block.Id);

        // Assert
        var actual = await _context.BlockCompletedInfos
            .SingleOrDefaultAsync(b => b.UserId == user.Id && b.BlockId == block.Id);
        Assert.NotNull(actual);
        Assert.Equal(user.Id, actual.UserId);
        Assert.Equal(block.Id, actual.BlockId);
    }

    [Fact]
    public async Task DeleteCompletedCourseBlocksAsync_ExistingBlock_DeletedSuccessfully()
    {
        // Arrange
        var blockCompletedInfo = await _context.BlockCompletedInfos.FirstAsync();

        // Act
        await _blockCompletedInfoRepository.DeleteCompletedCourseBlocksAsync(blockCompletedInfo);

        // Assert
        var actual = await _context.BlockCompletedInfos.FindAsync(blockCompletedInfo.UserId, blockCompletedInfo.BlockId);
        Assert.Null(actual);
    }
}
