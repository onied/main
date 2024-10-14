using AutoFixture;
using Courses.Data;
using Courses.Data.Models;
using Courses.Services;
using Courses.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Tests.Courses.Helpers;
using Task = System.Threading.Tasks.Task;

namespace Tests.Courses.UnitTests.ServiceTests;

public class BlockRepositoryTests
{
    private readonly Fixture _fixture = new();
    private readonly AppDbContext _context;
    private readonly IBlockRepository _blockRepository;

    private int _notExistingBlockId;
    private int _existingSummaryBlockId;
    private int _existingVideoBlockId;
    private int _existingTasksBlockId;

    public BlockRepositoryTests()
    {
        _context = AppDbContextTest.GetContext();
        _blockRepository = new BlockRepository(_context);
        ProduceTestData();
    }

    [Fact]
    public async Task GetSummaryBlock_ExistingId_ReturnsBlock()
    {
        // Act
        var actual = await _blockRepository.GetSummaryBlock(_existingSummaryBlockId);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(_existingSummaryBlockId, actual.Id);
    }

    [Fact]
    public async Task GetSummaryBlock_NonExistingId_ReturnsNull()
    {
        // Act
        var actual = await _blockRepository.GetSummaryBlock(_notExistingBlockId);

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public async Task GetVideoBlock_ExistingId_ReturnsBlock()
    {
        // Act
        var actual = await _blockRepository.GetVideoBlock(_existingVideoBlockId);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(_existingVideoBlockId, actual.Id);
    }

    [Fact]
    public async Task GetVideoBlock_NonExistingId_ReturnsNull()
    {
        // Act
        var actual = await _blockRepository.GetVideoBlock(_notExistingBlockId);

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public async Task GetTasksBlock_ExistingId_ReturnsBlock()
    {
        // Act
        var actual = await _blockRepository.GetTasksBlock(_existingTasksBlockId);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(_existingTasksBlockId, actual.Id);
    }

    [Fact]
    public async Task GetTasksBlock_NonExistingId_ReturnsNull()
    {
        // Act
        var actual = await _blockRepository.GetTasksBlock(_notExistingBlockId);

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public async Task AddBlockReturnIdAsync_BlockStoredAndIdReturned()
    {
        // Arrange
        var block = _fixture.Build<VideoBlock>()
            .With(b => b.Id, 0)
            .Create();

        // Act
        var returnedId = await _blockRepository.AddBlockReturnIdAsync(block);
        var actualBlock = await _context.VideoBlocks.SingleOrDefaultAsync(b => b.Id == returnedId);

        // Assert
        Assert.NotNull(actualBlock);
        Assert.Equal(block.Title, actualBlock.Title);
        Assert.Equal(returnedId, actualBlock.Id);
    }

    [Fact]
    public async Task AddBlockAsync_BlockStored()
    {
        // Arrange
        var block = _fixture.Build<SummaryBlock>()
            .With(b => b.Id, 0)
            .With(b => b.ModuleId, 1)
            .Create();

        // Act
        await _blockRepository.AddBlockAsync(block);
        var actualBlock = await _context.SummaryBlocks.SingleOrDefaultAsync(b => b.Id == block.Id);

        // Assert
        Assert.NotNull(actualBlock);
        Assert.Equal(block.Title, actualBlock.Title);
        Assert.Equal(block.ModuleId, actualBlock.ModuleId);
    }

    [Fact]
    public async Task UpdateSummaryBlock_BlockUpdated()
    {
        // Arrange
        var block = await _blockRepository.GetSummaryBlock(_existingSummaryBlockId);
        block!.Title = "Updated Summary Title";

        // Act
        await _blockRepository.UpdateSummaryBlock(block);
        var actualBlock = await _context.SummaryBlocks.SingleOrDefaultAsync(b => b.Id == _existingSummaryBlockId);

        // Assert
        Assert.NotNull(actualBlock);
        Assert.Equal("Updated Summary Title", actualBlock.Title);
    }

    [Fact]
    public async Task UpdateVideoBlock_BlockUpdated()
    {
        // Arrange
        var block = await _blockRepository.GetVideoBlock(_existingVideoBlockId);
        block!.Title = "Updated Video Title";

        // Act
        await _blockRepository.UpdateVideoBlock(block);
        var actualBlock = await _context.VideoBlocks.SingleOrDefaultAsync(b => b.Id == _existingVideoBlockId);

        // Assert
        Assert.NotNull(actualBlock);
        Assert.Equal("Updated Video Title", actualBlock.Title);
    }

    [Fact]
    public async Task UpdateTasksBlock_BlockUpdated()
    {
        // Arrange
        var block = await _blockRepository.GetTasksBlock(_existingTasksBlockId);
        block!.Title = "Updated Tasks Block Title";

        // Act
        await _blockRepository.UpdateTasksBlock(block);
        var actualBlock = await _context.TasksBlocks.SingleOrDefaultAsync(b => b.Id == _existingTasksBlockId);

        // Assert
        Assert.NotNull(actualBlock);
        Assert.Equal("Updated Tasks Block Title", actualBlock.Title);
    }

    [Fact]
    public async Task RenameBlockAsync_ExistingId_ChangesTitle()
    {
        // Arrange
        var newTitle = "Renamed Block Title";

        // Act
        var result = await _blockRepository.RenameBlockAsync(_existingSummaryBlockId, newTitle);
        var actualBlock = await _blockRepository.GetSummaryBlock(_existingSummaryBlockId);

        // Assert
        Assert.True(result);
        Assert.NotNull(actualBlock);
        Assert.Equal(newTitle, actualBlock.Title);
    }

    [Fact]
    public async Task RenameBlockAsync_NonExistingId_ReturnsFalse()
    {
        // Act
        var result = await _blockRepository.RenameBlockAsync(_notExistingBlockId, "Some Title");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteBlockAsync_ExistingId_BlockDeleted()
    {
        // Act
        var result = await _blockRepository.DeleteBlockAsync(_existingTasksBlockId);
        var actualBlock = await _blockRepository.GetTasksBlock(_existingTasksBlockId);

        // Assert
        Assert.True(result);
        Assert.Null(actualBlock);
    }

    [Fact]
    public async Task DeleteBlockAsync_NonExistingId_ReturnsFalse()
    {
        // Act
        var result = await _blockRepository.DeleteBlockAsync(_notExistingBlockId);

        // Assert
        Assert.False(result);
    }

    private void ProduceTestData()
    {
        var summaryBlock = _fixture.Build<SummaryBlock>()
            .With(b => b.Id, 1)
            .With(b => b.BlockType, BlockType.SummaryBlock)
            .Create();
        var videoBlock = _fixture.Build<VideoBlock>()
            .With(b => b.Id, 2)
            .With(b => b.BlockType, BlockType.VideoBlock)
            .Create();
        var tasksBlock = _fixture.Build<TasksBlock>()
            .With(b => b.Id, 3)
            .With(b => b.BlockType, BlockType.TasksBlock)
            .Create();

        _existingSummaryBlockId = summaryBlock.Id;
        _existingVideoBlockId = videoBlock.Id;
        _existingTasksBlockId = tasksBlock.Id;
        _notExistingBlockId = 999;

        _context.SummaryBlocks.Add(summaryBlock);
        _context.VideoBlocks.Add(videoBlock);
        _context.TasksBlocks.Add(tasksBlock);
        _context.SaveChanges();
    }
}
