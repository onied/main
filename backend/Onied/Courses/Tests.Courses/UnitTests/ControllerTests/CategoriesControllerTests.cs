using AutoFixture;
using AutoMapper;
using Courses.Controllers;
using Courses.Data.Models;
using Courses.Dtos.Catalog.Response;
using Courses.Handlers;
using Courses.Profiles;
using Courses.Queries;
using Courses.Services.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace Tests.Courses.UnitTests.ControllerTests;

public class CategoriesControllerTests
{
    private readonly Mock<ICategoryRepository> _categoryRepository = new();
    private readonly Mock<ISender> _sender = new();
    private readonly CategoriesController _controller;
    private readonly GetCategoriesQueryHandler _handler;
    private readonly Fixture _fixture = new();

    private readonly IMapper _mapper = new Mapper(
        new MapperConfiguration(cfg => cfg.AddProfile(new AppMappingProfile()))
    );

    public CategoriesControllerTests()
    {
        _controller = new CategoriesController(_sender.Object);
        _handler = new GetCategoriesQueryHandler(_mapper, _categoryRepository.Object);
    }

    [Fact]
    public async Task Get_ReturnsNothing()
    {
        // Arrange
        _sender
            .Setup(m => m.Send(It.IsAny<GetCategoriesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(It.IsAny<IResult>());
        _categoryRepository
            .Setup(r => r.GetAllCategoriesAsync())
            .Returns(Task.FromResult<List<Category>>([]));
        _sender
            .Setup(m => m.Send(It.IsAny<GetCategoriesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                await _handler.Handle(It.IsAny<GetCategoriesQuery>(), It.IsAny<CancellationToken>())
            );

        // Act

        var result = await _controller.GetCategories();
        var actualResult = result;

        // Assert
        Assert.NotNull(actualResult);
        //Assert.Empty(actualResult);
    }

    [Fact]
    public async Task Get_ReturnsListOfCategories()
    {
        // Arrange
        var categories = Enumerable
            .Range(1, 100)
            .Select(_ => _fixture.Build<Category>().Create())
            .ToList();
        var categoriesMapped = _mapper.Map<List<CategoryResponse>>(categories);
        _categoryRepository
            .Setup(r => r.GetAllCategoriesAsync())
            .Returns(Task.FromResult(categories));
        _sender
            .Setup(m => m.Send(It.IsAny<GetCategoriesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                await _handler.Handle(It.IsAny<GetCategoriesQuery>(), It.IsAny<CancellationToken>())
            );

        // Assert
        var result = Assert.IsType<Ok<List<CategoryResponse>>>(await _controller.GetCategories());
        Assert.NotNull(result.Value);
        Assert.Equivalent(categoriesMapped, result.Value);
    }
}
