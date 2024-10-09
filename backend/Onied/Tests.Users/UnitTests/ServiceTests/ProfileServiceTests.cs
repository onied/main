using System.Security.Claims;
using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Tests.Users.UnitTests.Helpers;
using Users.Data.Entities;
using Users.Dtos.Profile.Request;
using Users.Dtos.Users.Response;
using Users.Profiles;
using Users.Services.ProfileProducer;
using Users.Services.ProfileService;

namespace Tests.Users.UnitTests.ServiceTests;

public class ProfileServiceTests
{
    private readonly ProfileService _service;
    private readonly Fixture _fixture = new();
    private readonly Mock<IProfileProducer> _profileProducer = new();
    private readonly IMapper _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AppMappingProfile())));
    private readonly Mock<TestUserManager> _userManager = new();

    public ProfileServiceTests()
    {
        _service = new ProfileService(_profileProducer.Object, _userManager.Object, _mapper);
    }

    [Fact]
    public async Task Get_ReturnsUnauthorized_WhenCannotGetUser()
    {
        // Arrange
        _userManager.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(null as AppUser);

        // Act
        var result = await _service.Get(new ClaimsPrincipal());

        // Assert
        Assert.IsType<UnauthorizedHttpResult>(result);
    }

    [Fact]
    public async Task Get_ReturnsOk_WhenUserExists()
    {
        // Arrange
        var user = _fixture.Create<AppUser>();
        _userManager.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

        // Act
        var result = await _service.Get(new ClaimsPrincipal());

        // Assert
        Assert.IsType<Ok<UserProfileResponse>>(result);
        var profile = (result as Ok<UserProfileResponse>)!.Value!;
        Assert.Equal(user.FirstName, profile.FirstName);
        Assert.Equal(user.LastName, profile.LastName);
        Assert.Equal(user.Gender, profile.Gender);
        Assert.Equal(user.Avatar, profile.Avatar);
        Assert.Equal(user.Email, profile.Email);
    }

    [Fact]
    public async Task EditProfile_ReturnsOk_WhenUserExists()
    {
        // Arrange
        var request = _fixture.Create<ProfileChangedRequest>();
        var user = _fixture.Create<AppUser>();
        _userManager.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        AppUser changedUser = null!;
        _userManager.Setup(manager => manager.UpdateAsync(It.IsAny<AppUser>())).Callback((AppUser u) => changedUser = u);
        var published = false;
        _profileProducer.Setup(producer => producer.PublishProfileUpdatedAsync(It.IsAny<AppUser>()))
            .Callback(() => published = true);

        // Act
        var result = await _service.EditProfile(request, new ClaimsPrincipal());

        // Assert
        Assert.IsType<Ok>(result);
        Assert.NotNull(changedUser);
        Assert.True(published);
        Assert.Equal(request.Gender, changedUser.Gender);
        Assert.Equal(request.FirstName, changedUser.FirstName);
        Assert.Equal(request.LastName, changedUser.LastName);
    }

    [Fact]
    public async Task EditProfile_ReturnsUnauthorized_WhenCannotGetUser()
    {

        // Arrange
        var request = _fixture.Create<ProfileChangedRequest>();
        _userManager.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(null as AppUser);

        // Act
        var result = await _service.EditProfile(request, new ClaimsPrincipal());

        // Assert
        Assert.IsType<UnauthorizedHttpResult>(result);
    }

    [Fact]
    public async Task Avatar_ReturnsUnauthorized_WhenCannotGetUser()
    {
        // Arrange
        var request = _fixture.Create<AvatarChangedRequest>();
        _userManager.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(null as AppUser);

        // Act
        var result = await _service.Avatar(request, new ClaimsPrincipal());

        // Assert
        Assert.IsType<UnauthorizedHttpResult>(result);
    }

    [Fact]
    public async Task Avatar_ReturnsOk_WhenUserExists()
    {
        // Arrange
        var request = _fixture.Create<AvatarChangedRequest>();
        var user = _fixture.Create<AppUser>();
        _userManager.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        AppUser changedUser = null!;
        _userManager.Setup(manager => manager.UpdateAsync(It.IsAny<AppUser>())).Callback((AppUser u) => changedUser = u);
        var published = false;
        _profileProducer.Setup(producer => producer.PublishProfilePhotoUpdatedAsync(It.IsAny<AppUser>()))
            .Callback(() => published = true);

        // Act
        var result = await _service.Avatar(request, new ClaimsPrincipal());

        // Assert
        Assert.IsType<Ok>(result);
        Assert.NotNull(changedUser);
        Assert.True(published);
        Assert.Equal(request.AvatarHref, changedUser.Avatar);
    }
}
