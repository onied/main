using AutoFixture;
using Courses.Data;
using Courses.Data.Models;
using Courses.Services;
using Courses.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Tests.Courses.Helpers;
using Task = System.Threading.Tasks.Task;

namespace Tests.Courses.UnitTests.ServiceTests;

public class ModuleRepositoryTests
{
    private readonly Fixture _fixture = new();
    private readonly AppDbContext _context;
    private readonly IModuleRepository _moduleRepository;

    private int _existingModuleId;
    private int _notExistingModuleId;
    private Module _module = null!;

    public ModuleRepositoryTests()
    {
        _context = AppDbContextTest.GetContext();
        _moduleRepository = new ModuleRepository(_context);
        ProduceTestData();
    }

    [Fact]
    public async Task GetModuleAsync_ExistingId_ReturnsModule()
    {
        // Act
        var module = await _moduleRepository.GetModuleAsync(_existingModuleId);

        // Assert
        Assert.NotNull(module);
        Assert.Equal(_existingModuleId, module.Id);
    }

    [Fact]
    public async Task GetModuleAsync_NonExistingId_ReturnsNull()
    {
        // Act
        var module = await _moduleRepository.GetModuleAsync(_notExistingModuleId);

        // Assert
        Assert.Null(module);
    }

    [Fact]
    public async Task AddModuleAsync_ModuleStored()
    {
        // Arrange
        var newModule = _fixture.Build<Module>()
            .With(m => m.Id, 0)
            .Create();

        // Act
        await _moduleRepository.AddModuleAsync(newModule);
        var actualModule = await _context.Modules.SingleOrDefaultAsync(m => m.Title == newModule.Title);

        // Assert
        Assert.NotNull(actualModule);
        Assert.Equal(newModule.Title, actualModule.Title);
    }

    [Fact]
    public async Task AddModuleReturnIdAsync_ModuleStoredAndIdReturned()
    {
        // Arrange
        var newModule = _fixture.Build<Module>()
            .With(m => m.Id, 0)
            .Create();

        // Act
        var returnedId = await _moduleRepository.AddModuleReturnIdAsync(newModule);

        // Assert
        Assert.NotEqual(0, returnedId);
        var actualModule = await _context.Modules.FindAsync(returnedId);
        Assert.NotNull(actualModule);
        Assert.Equal(newModule.Title, actualModule.Title);
    }

    [Fact]
    public async Task UpdateModuleAsync_ModuleUpdated()
    {
        // Arrange
        _module.Title = "Updated Title";

        // Act
        await _moduleRepository.UpdateModuleAsync(_module);
        var updatedModule = await _context.Modules.FindAsync(_module.Id);

        // Assert
        Assert.NotNull(updatedModule);
        Assert.Equal("Updated Title", updatedModule.Title);
    }

    [Fact]
    public async Task RenameModuleAsync_ExistingId_RenamesModule()
    {
        // Act
        var result = await _moduleRepository.RenameModuleAsync(_existingModuleId, "Renamed Module");

        // Assert
        Assert.True(result);
        var renamedModule = await _context.Modules.FindAsync(_existingModuleId);
        Assert.NotNull(renamedModule);
        Assert.Equal("Renamed Module", renamedModule.Title);
    }

    [Fact]
    public async Task RenameModuleAsync_NonExistingId_ReturnsFalse()
    {
        // Act
        var result = await _moduleRepository.RenameModuleAsync(999, "Renamed Module");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteModuleAsync_ExistingId_ModuleDeleted()
    {
        // Act
        var result = await _moduleRepository.DeleteModuleAsync(_existingModuleId);

        // Assert
        Assert.True(result);
        var deletedModule = await _context.Modules.FindAsync(_existingModuleId);
        Assert.Null(deletedModule);
    }

    [Fact]
    public async Task DeleteModuleAsync_NonExistingId_ReturnsFalse()
    {
        // Act
        var result = await _moduleRepository.DeleteModuleAsync(999);

        // Assert
        Assert.False(result);
    }

    private void ProduceTestData()
    {
        _module = _fixture.Build<Module>()
            .With(m => m.Id, 1)
            .With(m => m.CourseId, 1)
            .With(m => m.Title, "Test Module")
            .Create();

        _existingModuleId = _module.Id;
        _notExistingModuleId = 999;

        _context.Modules.Add(_module);
        _context.SaveChanges();
    }
}
