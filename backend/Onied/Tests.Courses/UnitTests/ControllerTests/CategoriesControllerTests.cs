using AutoFixture;
using AutoMapper;
using Courses.Controllers;
using Courses.Dtos;
using Courses.Models;
using Courses.Profiles;
using Courses.Services;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace Tests.Courses.UnitTests.ControllerTests;

public class CategoriesControllerTests
{
    private readonly Mock<ICategoryRepository> _categoryRepository = new();
    private readonly CategoriesController _controller;
    private readonly Fixture _fixture = new();

    private readonly IMapper _mapper =
        new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AppMappingProfile())));

    public CategoriesControllerTests()
    {
        _controller = new CategoriesController(_mapper, _categoryRepository.Object);
    }

    [Fact]
    public async Task Get_ReturnsNothing()
    {
        // Arrange

        _categoryRepository.Setup(r => r.GetAllCategoriesAsync())
            .Returns(Task.FromResult<List<Category>>([]));

        // Act

        var result = await _controller.GetCategories();
        var actualResult = result.Value;

        // Assert
        Assert.NotNull(actualResult);
        Assert.Empty(actualResult);
    }

    [Fact]
    public async Task Get_ReturnsListOfCategories()
    {
        // Arrange
        var categories = Enumerable.Range(1, 100).Select(_ => _fixture.Build<Category>().Create()).ToList();
        var categoriesMapped = _mapper.Map<List<CategoryDto>>(categories);
        _categoryRepository.Setup(r => r.GetAllCategoriesAsync())
            .Returns(Task.FromResult(categories));

        // Act

        var result = await _controller.GetCategories();
        var actualResult = result.Value;

        // Assert
        Assert.NotNull(actualResult);
        Assert.Equivalent(categoriesMapped, actualResult);
    }
}
