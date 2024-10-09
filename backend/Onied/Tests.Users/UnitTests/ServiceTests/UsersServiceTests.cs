using System.Diagnostics;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Web;
using AutoFixture;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Tests.Users.UnitTests.Helpers;
using Users.Data.Entities;
using Users.Data.Enums;
using Users.Dtos.Users.Request;
using Users.Dtos.VkOauth.Request;
using Users.Services.UserCreatedProducer;
using Users.Services.UsersService;

namespace Tests.Users.UnitTests.ServiceTests;

public class UsersServiceTests
{
    private readonly Mock<IOptionsMonitor<BearerTokenOptions>> _bearerTokenOptions = new();
    private readonly Mock<IHttpClientFactory> _clientFactory = new();

    private readonly Dictionary<string, string> _config = new()
    {
        { "Authentication:VK:ClientId", "123123" },
        { "Authentication:VK:ClientSecret", "asdlfajslkdfjaskdlf" },
        { "ConnectionStrings:VkAccessTokenUri", "http://localhost" },
        { "ConnectionStrings:VkGetProfileInfoMethodUri", "http://localhost" },
        { "VkApiVersion", "0" }
    };

    private readonly IConfiguration _configuration;
    private readonly Mock<IEmailSender<AppUser>> _emailSender = new();
    private readonly Fixture _fixture = new();
    private readonly Mock<LinkGenerator> _linkGenerator = new();
    private readonly UsersService _service;
    private readonly Mock<TestSignInManager> _signInManager = new();
    private readonly Mock<TimeProvider> _timeProvider = new();
    private readonly Mock<IUserCreatedProducer> _userCreatedProducer = new();
    private readonly Mock<TestUserManager> _userManager = new();
    private readonly Mock<IUserStore<AppUser>> _userStore = new();

    public UsersServiceTests()
    {
        _configuration = new ConfigurationBuilder().AddInMemoryCollection(_config!).Build();
        _userStore.As<IUserEmailStore<AppUser>>();
        _service = new UsersService(_userManager.Object, _userStore.Object, _linkGenerator.Object, _emailSender.Object,
            _userCreatedProducer.Object, _signInManager.Object, _bearerTokenOptions.Object, _timeProvider.Object,
            _clientFactory.Object, _configuration);
    }

    [Fact]
    public async Task Register_ThrowsNotSupported_WhenUserManagerDoesNotSupportUserMail()
    {
        // Arrange
        var registration = _fixture.Create<RegisterUserRequest>();
        _userManager.SetupGet(manager => manager.SupportsUserEmail).Returns(false);

        // Act
        Task<IResult> Act()
        {
            return _service.Register(registration, new DefaultHttpContext());
        }

        // Assert
        await Assert.ThrowsAsync<NotSupportedException>(Act);
    }

    [Theory]
    [InlineData("")]
    [InlineData("the")]
    [InlineData("a@")]
    [InlineData("@qwerty")]
    public async Task Register_ReturnsValidationProblem_WhenEmailInvalid(string email)
    {
        // Arrange
        var registration = _fixture.Create<RegisterUserRequest>();
        registration.Email = email;
        _userManager.SetupGet(manager => manager.SupportsUserEmail).Returns(true);

        // Act
        var result = await _service.Register(registration, new DefaultHttpContext());

        // Assert
        Assert.IsType<ValidationProblem>(result);
        var validationResult = (result as ValidationProblem)!;
        Assert.Equal(400, validationResult.StatusCode);
        Assert.Contains(email, validationResult.ProblemDetails.Errors.First().Value.First());
    }

    [Fact]
    public async Task Register_ReturnsValidationProblem_WhenCouldNotRegisterUser()
    {
        // Arrange
        var registration = _fixture.Create<RegisterUserRequest>();
        registration.Email = "asdf@asdf.asdf";
        _userManager.SetupGet(manager => manager.SupportsUserEmail).Returns(true);
        var error = "Descriptive and informative error message";
        _userManager.Setup(manager =>
            manager.CreateAsync(
                It.IsAny<AppUser>(),
                registration.Password)).ReturnsAsync(IdentityResult.Failed(new IdentityError
                {
                    Code = "",
                    Description = error
                }));

        // Act
        var result = await _service.Register(registration, new DefaultHttpContext());

        // Assert
        Assert.IsType<ValidationProblem>(result);
        var validationResult = (result as ValidationProblem)!;
        Assert.Equal(400, validationResult.StatusCode);
        Assert.Equivalent(error, validationResult.ProblemDetails.Errors.First().Value.First());
    }

    [Fact]
    public async Task Register_ReturnsOk_WhenEmailCorrect()
    {
        // Arrange
        var registration = _fixture.Create<RegisterUserRequest>();
        registration.Email = "asdf@asdf.asdf";
        _userManager.SetupGet(manager => manager.SupportsUserEmail).Returns(true);
        _userManager.Setup(manager =>
            manager.CreateAsync(
                It.IsAny<AppUser>(),
                registration.Password)).ReturnsAsync(IdentityResult.Success);
        AppUser user = null!;
        _userCreatedProducer.Setup(producer => producer.PublishAsync(It.IsAny<AppUser>()))
            .Callback<AppUser>(u => user = u);
        _userManager.Setup(manager => manager.GenerateEmailConfirmationTokenAsync(It.IsAny<AppUser>()))
            .ReturnsAsync("code");
        var response = "Nothing really matters";
        _linkGenerator.Setup(m => m.GetUriByAddress(It.IsAny<HttpContext>(), It.IsAny<RouteValuesAddress>(),
                It.IsAny<RouteValueDictionary>(), default, default, default, default, default, default))
            .Returns(response);
        string confirmationUrl = null!;
        string userEmail = null!;
        _emailSender
            .Setup(sender =>
                sender.SendConfirmationLinkAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>())).Callback(
                (AppUser _, string email, string confirmation) =>
                {
                    userEmail = email;
                    confirmationUrl = confirmation;
                });

        // Act
        var result = await _service.Register(registration, new DefaultHttpContext());

        // Assert
        Assert.IsType<Ok>(result);
        Assert.Equal(registration.Gender, user.Gender);
        Assert.Equal(registration.FirstName, user.FirstName);
        Assert.Equal(registration.LastName, user.LastName);
        Assert.False(user.EmailConfirmed);
        Assert.Equal(registration.Email, userEmail);
        Assert.Contains(HtmlEncoder.Default.Encode(response), confirmationUrl);
    }


    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenPasswordIncorrect()
    {
        // Arrange
        var login = _fixture.Create<LoginRequest>();
        _signInManager.Setup(manager => manager.PasswordSignInAsync(login.Email, login.Password, false, true))
            .ReturnsAsync(SignInResult.Failed);

        // Act
        var result = await _service.Login(login);

        // Assert
        Assert.IsType<ProblemHttpResult>(result);
        var problem = (result as ProblemHttpResult)!;
        Assert.Equal(StatusCodes.Status401Unauthorized, problem.StatusCode);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenTwoFactorCodeIncorrect()
    {
        // Arrange
        var login = _fixture.Build<LoginRequest>().With(request => request.TwoFactorCode, () => "Not Null Or Empty")
            .Create();
        _signInManager.Setup(manager => manager.PasswordSignInAsync(login.Email, login.Password, false, true))
            .ReturnsAsync(SignInResult.TwoFactorRequired);
        _signInManager.Setup(manager => manager.TwoFactorAuthenticatorSignInAsync(login.TwoFactorCode!, false, false))
            .ReturnsAsync(SignInResult.Failed);

        // Act
        var result = await _service.Login(login);

        // Assert
        Assert.IsType<ProblemHttpResult>(result);
        var problem = (result as ProblemHttpResult)!;
        Assert.Equal(StatusCodes.Status401Unauthorized, problem.StatusCode);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenTwoFactorRecoveryCodeIncorrect()
    {
        // Arrange
        var login = _fixture.Build<LoginRequest>().With(request => request.TwoFactorCode, () => "")
            .With(request => request.TwoFactorRecoveryCode, () => "Not null or empty")
            .Create();
        _signInManager.Setup(manager => manager.PasswordSignInAsync(login.Email, login.Password, false, true))
            .ReturnsAsync(SignInResult.TwoFactorRequired);
        _signInManager.Setup(manager => manager.TwoFactorAuthenticatorSignInAsync(login.TwoFactorCode!, false, false))
            .Callback(() => throw new UnreachableException());
        _signInManager.Setup(manager =>
                manager.TwoFactorRecoveryCodeSignInAsync(login.TwoFactorRecoveryCode!))
            .ReturnsAsync(SignInResult.Failed);

        // Act
        var result = await _service.Login(login);

        // Assert
        Assert.IsType<ProblemHttpResult>(result);
        var problem = (result as ProblemHttpResult)!;
        Assert.Equal(StatusCodes.Status401Unauthorized, problem.StatusCode);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenTwoFactorNotProvided()
    {
        // Arrange
        var login = _fixture.Build<LoginRequest>().With(request => request.TwoFactorCode, () => "")
            .With(request => request.TwoFactorRecoveryCode, () => "")
            .Create();
        _signInManager.Setup(manager => manager.PasswordSignInAsync(login.Email, login.Password, false, true))
            .ReturnsAsync(SignInResult.TwoFactorRequired);
        _signInManager.Setup(manager => manager.TwoFactorAuthenticatorSignInAsync(login.TwoFactorCode!, false, false))
            .Callback(() => throw new UnreachableException());
        _signInManager.Setup(manager => manager.TwoFactorRecoveryCodeSignInAsync(login.TwoFactorRecoveryCode!))
            .Callback(() => throw new UnreachableException());

        // Act
        var result = await _service.Login(login);

        // Assert
        Assert.IsType<ProblemHttpResult>(result);
        var problem = (result as ProblemHttpResult)!;
        Assert.Equal(StatusCodes.Status401Unauthorized, problem.StatusCode);
    }

    [Fact]
    public async Task Login_ReturnsEmpty_WhenPasswordCorrectAndTwoFactorNotRequired()
    {
        // Arrange
        var login = _fixture.Create<LoginRequest>();
        _signInManager.Setup(manager => manager.PasswordSignInAsync(login.Email, login.Password, false, true))
            .ReturnsAsync(SignInResult.Success);

        // Act
        var result = await _service.Login(login);

        // Assert
        Assert.IsType<EmptyHttpResult>(result);
    }


    [Fact]
    public async Task Login_ReturnsEmpty_WhenPasswordAndTwoFactorCodeCorrect()
    {
        // Arrange
        var login = _fixture.Build<LoginRequest>().With(request => request.TwoFactorCode, () => "Not null or empty")
            .Create();
        _signInManager.Setup(manager => manager.PasswordSignInAsync(login.Email, login.Password, false, true))
            .ReturnsAsync(SignInResult.TwoFactorRequired);
        _signInManager.Setup(manager => manager.TwoFactorAuthenticatorSignInAsync(login.TwoFactorCode!, false, false))
            .ReturnsAsync(SignInResult.Success);

        // Act
        var result = await _service.Login(login);

        // Assert
        Assert.IsType<EmptyHttpResult>(result);
    }

    [Fact]
    public async Task Login_ReturnsEmpty_WhenPasswordAndRecoveryCodeCorrect()
    {
        // Arrange
        var login = _fixture.Build<LoginRequest>().With(request => request.TwoFactorCode, () => "")
            .With(request => request.TwoFactorRecoveryCode, () => "Not null or empty")
            .Create();
        _signInManager.Setup(manager => manager.PasswordSignInAsync(login.Email, login.Password, false, true))
            .ReturnsAsync(SignInResult.TwoFactorRequired);
        _signInManager.Setup(manager => manager.TwoFactorAuthenticatorSignInAsync(login.TwoFactorCode!, false, false))
            .Callback(() => throw new UnreachableException());
        _signInManager.Setup(manager =>
                manager.TwoFactorRecoveryCodeSignInAsync(login.TwoFactorRecoveryCode!))
            .ReturnsAsync(SignInResult.Success);

        // Act
        var result = await _service.Login(login);

        // Assert
        Assert.IsType<EmptyHttpResult>(result);
    }

    [Fact]
    public async Task Refresh_ReturnsUnauthorized_WhenTokenHasNoExpiration()
    {
        // Arrange
        var refresh = _fixture.Create<RefreshRequest>();
        var protector = new Mock<ISecureDataFormat<AuthenticationTicket>>();
        var principal = _fixture.Create<ClaimsPrincipal>();
        var ticket = new AuthenticationTicket(principal, IdentityConstants.BearerScheme)
        {
            Properties =
            {
                ExpiresUtc = null
            }
        };
        protector.Setup(p => p.Unprotect(refresh.RefreshToken)).Returns(ticket);
        var anotherBearerTokenOptions = new BearerTokenOptions
        {
            RefreshTokenProtector = protector.Object
        };
        _bearerTokenOptions.Setup(options => options.Get(IdentityConstants.BearerScheme))
            .Returns(anotherBearerTokenOptions);

        // Act
        var result = await _service.Refresh(refresh);

        // Assert
        Assert.IsType<ChallengeHttpResult>(result);
    }

    [Fact]
    public async Task Refresh_ReturnsUnauthorized_WhenTokenExpired()
    {
        // Arrange
        var refresh = _fixture.Create<RefreshRequest>();
        var protector = new Mock<ISecureDataFormat<AuthenticationTicket>>();
        var principal = _fixture.Create<ClaimsPrincipal>();
        _timeProvider.Setup(provider => provider.GetUtcNow()).Returns(DateTimeOffset.Parse("2009-09-09T09:09:09Z"));
        var ticket = new AuthenticationTicket(principal, IdentityConstants.BearerScheme)
        {
            Properties =
            {
                ExpiresUtc = DateTimeOffset.UnixEpoch
            }
        };
        protector.Setup(p => p.Unprotect(refresh.RefreshToken)).Returns(ticket);
        var anotherBearerTokenOptions = new BearerTokenOptions
        {
            RefreshTokenProtector = protector.Object
        };
        _bearerTokenOptions.Setup(options => options.Get(IdentityConstants.BearerScheme))
            .Returns(anotherBearerTokenOptions);

        // Act
        var result = await _service.Refresh(refresh);

        // Assert
        Assert.IsType<ChallengeHttpResult>(result);
    }

    [Fact]
    public async Task Refresh_ReturnsUnauthorized_WhenTokenNotValid()
    {
        // Arrange
        var refresh = _fixture.Create<RefreshRequest>();
        var protector = new Mock<ISecureDataFormat<AuthenticationTicket>>();
        var principal = _fixture.Create<ClaimsPrincipal>();
        var now = DateTimeOffset.Parse("2009-09-09T09:09:09Z");
        _timeProvider.Setup(provider => provider.GetUtcNow()).Returns(now);
        var ticket = new AuthenticationTicket(principal, IdentityConstants.BearerScheme)
        {
            Properties =
            {
                ExpiresUtc = now.AddDays(1)
            }
        };
        protector.Setup(p => p.Unprotect(refresh.RefreshToken)).Returns(ticket);
        var anotherBearerTokenOptions = new BearerTokenOptions
        {
            RefreshTokenProtector = protector.Object
        };
        _bearerTokenOptions.Setup(options => options.Get(IdentityConstants.BearerScheme))
            .Returns(anotherBearerTokenOptions);
        _signInManager.Setup(manager => manager.ValidateSecurityStampAsync(principal)).ReturnsAsync(null as AppUser);

        // Act
        var result = await _service.Refresh(refresh);

        // Assert
        Assert.IsType<ChallengeHttpResult>(result);
    }

    [Fact]
    public async Task Refresh_ReturnsSignIn_WhenTokenValid()
    {
        // Arrange
        var refresh = _fixture.Create<RefreshRequest>();
        var protector = new Mock<ISecureDataFormat<AuthenticationTicket>>();
        var principal = _fixture.Create<ClaimsPrincipal>();
        var now = DateTimeOffset.Parse("2009-09-09T09:09:09Z");
        _timeProvider.Setup(provider => provider.GetUtcNow()).Returns(now);
        var ticket = new AuthenticationTicket(principal, IdentityConstants.BearerScheme)
        {
            Properties =
            {
                ExpiresUtc = now.AddDays(1)
            }
        };
        protector.Setup(p => p.Unprotect(refresh.RefreshToken)).Returns(ticket);
        var anotherBearerTokenOptions = new BearerTokenOptions
        {
            RefreshTokenProtector = protector.Object
        };
        _bearerTokenOptions.Setup(options => options.Get(IdentityConstants.BearerScheme))
            .Returns(anotherBearerTokenOptions);
        var user = _fixture.Create<AppUser>();
        _signInManager.Setup(manager => manager.ValidateSecurityStampAsync(principal)).ReturnsAsync(user);
        _signInManager.Setup(manager => manager.CreateUserPrincipalAsync(user)).ReturnsAsync(principal);

        // Act
        var result = await _service.Refresh(refresh);

        // Assert
        Assert.IsType<SignInHttpResult>(result);
        var signIn = (result as SignInHttpResult)!;
        Assert.Equal(principal, signIn.Principal);
        Assert.Equal(IdentityConstants.BearerScheme, signIn.AuthenticationScheme);
    }

    [Fact]
    public async Task ConfirmEmail_ReturnsUnauthorized_WhenUserNotFound()
    {
        // Arrange
        var user = _fixture.Create<AppUser>();
        const string code64 = "UGVhY2g=";
        _userManager.Setup(manager => manager.FindByIdAsync(user.Id)).ReturnsAsync(null as AppUser);

        // Act
        var result = await _service.ConfirmEmail(user.Id, code64, null);

        // Assert
        Assert.IsType<UnauthorizedHttpResult>(result);
    }

    [Fact]
    public async Task ConfirmEmail_ReturnsUnauthorized_WhenCodeNotBase64()
    {
        // Arrange
        var user = _fixture.Create<AppUser>();
        const string code64 = "Peach";
        _userManager.Setup(manager => manager.FindByIdAsync(user.Id!)).ReturnsAsync(user);

        // Act
        var result = await _service.ConfirmEmail(user.Id, code64, null);

        // Assert
        Assert.IsType<UnauthorizedHttpResult>(result);
    }


    [Fact]
    public async Task ConfirmEmail_ReturnsUnauthorized_WhenCodeIncorrect()
    {
        // Arrange
        var user = _fixture.Create<AppUser>();
        const string code64 = "UGVhY2g=";
        const string code = "Peach";
        _userManager.Setup(manager => manager.FindByIdAsync(user.Id)).ReturnsAsync(user);
        _userManager.Setup(manager => manager.ConfirmEmailAsync(user, code))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError
            {
                Code = "error",
                Description = "meat"
            }));

        // Act
        var result = await _service.ConfirmEmail(user.Id, code64, null);

        // Assert
        Assert.IsType<UnauthorizedHttpResult>(result);
    }


    [Fact]
    public async Task ConfirmEmail_ReturnsUnauthorized_WhenChangingEmailFailed()
    {
        // Arrange
        var user = _fixture.Create<AppUser>();
        const string code64 = "UGVhY2g=";
        const string code = "Peach";
        const string changedEmail = "asdf@asdf.asdf";
        _userManager.Setup(manager => manager.FindByIdAsync(user.Id)).ReturnsAsync(user);
        _userManager.Setup(manager => manager.ConfirmEmailAsync(user, code))
            .Callback(() => throw new UnreachableException());
        _userManager.Setup(manager => manager.ChangeEmailAsync(user, changedEmail, code)).ReturnsAsync(
            IdentityResult.Failed(new IdentityError
            {
                Code = "error",
                Description = "meow"
            }));

        // Act
        var result = await _service.ConfirmEmail(user.Id, code64, changedEmail);

        // Assert
        Assert.IsType<UnauthorizedHttpResult>(result);
    }

    [Fact]
    public async Task ConfirmEmail_ReturnsOkAndChangesUsername_WhenChangingEmailSucceeded()
    {
        // Arrange
        var user = _fixture.Create<AppUser>();
        const string code64 = "UGVhY2g=";
        const string code = "Peach";
        const string changedEmail = "asdf@asdf.asdf";
        _userManager.Setup(manager => manager.FindByIdAsync(user.Id)).ReturnsAsync(user);
        _userManager.Setup(manager => manager.ConfirmEmailAsync(user, code))
            .Callback(() => throw new UnreachableException());
        _userManager.Setup(manager => manager.ChangeEmailAsync(user, changedEmail, code))
            .ReturnsAsync(IdentityResult.Success);
        var setUserNameCalled = false;
        _userManager.Setup(manager => manager.SetUserNameAsync(user, changedEmail))
            .Callback(() => setUserNameCalled = true)
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _service.ConfirmEmail(user.Id, code64, changedEmail);

        // Assert
        Assert.IsType<ContentHttpResult>(result);
        Assert.True(setUserNameCalled);
    }

    [Fact]
    public async Task ConfirmEmail_ReturnsOk_WhenConfirmSuccessful()
    {
        // Arrange
        var user = _fixture.Create<AppUser>();
        const string code64 = "UGVhY2g=";
        const string code = "Peach";
        _userManager.Setup(manager => manager.FindByIdAsync(user.Id)).ReturnsAsync(user);
        _userManager.Setup(manager => manager.ConfirmEmailAsync(user, code)).ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _service.ConfirmEmail(user.Id, code64, null);

        // Assert
        Assert.IsType<ContentHttpResult>(result);
    }

    [Fact]
    public async Task ResendConfirmationEmail_ReturnsOkButDoesNotSendConfirmation_WhenEmailNotFound()
    {
        // Arrange
        var resend = _fixture.Create<ResendConfirmationEmailRequest>();
        _userManager.Setup(manager => manager.FindByEmailAsync(resend.Email)).ReturnsAsync(null as AppUser);
        const string response = "Nothing really matters";
        var sentConfirmation = false;
        _userManager.Setup(manager => manager.GenerateEmailConfirmationTokenAsync(It.IsAny<AppUser>()))
            .ReturnsAsync("code");
        _linkGenerator.Setup(m => m.GetUriByAddress(It.IsAny<HttpContext>(), It.IsAny<RouteValuesAddress>(),
                It.IsAny<RouteValueDictionary>(), default, default, default, default, default, default))
            .Callback(() => sentConfirmation = true)
            .Returns(response);

        // Act
        var result = await _service.ResendConfirmationEmail(resend, new DefaultHttpContext());

        // Assert
        Assert.IsType<Ok>(result);
        Assert.False(sentConfirmation);
    }

    [Fact]
    public async Task ResendConfirmationEmail_ReturnsOkAndSendsConfirmation_WhenEmailFound()
    {
        // Arrange
        var resend = _fixture.Create<ResendConfirmationEmailRequest>();
        var user = _fixture.Create<AppUser>();
        _userManager.Setup(manager => manager.FindByEmailAsync(resend.Email)).ReturnsAsync(user);
        const string response = "Nothing really matters";
        var sentConfirmation = false;
        _userManager.Setup(manager => manager.GenerateEmailConfirmationTokenAsync(It.IsAny<AppUser>()))
            .ReturnsAsync("code");
        _linkGenerator.Setup(m => m.GetUriByAddress(It.IsAny<HttpContext>(), It.IsAny<RouteValuesAddress>(),
                It.IsAny<RouteValueDictionary>(), default, default, default, default, default, default))
            .Callback(() => sentConfirmation = true)
            .Returns(response);

        // Act
        var result = await _service.ResendConfirmationEmail(resend, new DefaultHttpContext());

        // Assert
        Assert.IsType<Ok>(result);
        Assert.True(sentConfirmation);
    }

    [Fact]
    public async Task ForgotPassword_ReturnsOkButDoesNotSendCode_WhenUserDoesNotExist()
    {
        // Arrange
        var forgot = _fixture.Create<ForgotPasswordRequest>();
        _userManager.Setup(manager => manager.FindByEmailAsync(forgot.Email)).ReturnsAsync(null as AppUser);
        var sentPasswordResetCode = false;
        _emailSender
            .Setup(sender =>
                sender.SendPasswordResetCodeAsync(It.IsAny<AppUser>(), forgot.Email, It.IsAny<string>()))
            .Callback(() => sentPasswordResetCode = true);
        _userManager.Setup(manager => manager.GeneratePasswordResetTokenAsync(It.IsAny<AppUser>()))
            .ReturnsAsync("code");

        // Act
        var result = await _service.ForgotPassword(forgot);

        // Assert
        Assert.IsType<Ok>(result);
        Assert.False(sentPasswordResetCode);
    }

    [Fact]
    public async Task ForgotPassword_ReturnsOkButDoesNotSendCode_WhenUserNotConfirmed()
    {
        // Arrange
        var user = _fixture.Create<AppUser>();
        var forgot = _fixture.Build<ForgotPasswordRequest>().With(request => request.Email, user.Email).Create();
        _userManager.Setup(manager => manager.FindByEmailAsync(forgot.Email)).ReturnsAsync(user);
        var sentPasswordResetCode = false;
        _emailSender
            .Setup(sender =>
                sender.SendPasswordResetCodeAsync(It.IsAny<AppUser>(), forgot.Email, It.IsAny<string>()))
            .Callback(() => sentPasswordResetCode = true);
        _userManager.Setup(manager => manager.IsEmailConfirmedAsync(It.IsAny<AppUser>())).ReturnsAsync(false);
        _userManager.Setup(manager => manager.GeneratePasswordResetTokenAsync(It.IsAny<AppUser>()))
            .ReturnsAsync("code");

        // Act
        var result = await _service.ForgotPassword(forgot);

        // Assert
        Assert.IsType<Ok>(result);
        Assert.False(sentPasswordResetCode);
    }

    [Fact]
    public async Task ForgotPassword_ReturnsOkAndSendsCode_WhenUserConfirmed()
    {
        // Arrange
        var user = _fixture.Create<AppUser>();
        var forgot = _fixture.Build<ForgotPasswordRequest>().With(request => request.Email, user.Email).Create();
        _userManager.Setup(manager => manager.FindByEmailAsync(forgot.Email)).ReturnsAsync(user);
        var sentPasswordResetCode = false;
        _emailSender
            .Setup(sender =>
                sender.SendPasswordResetCodeAsync(It.IsAny<AppUser>(), forgot.Email, It.IsAny<string>()))
            .Callback(() => sentPasswordResetCode = true);
        _userManager.Setup(manager => manager.IsEmailConfirmedAsync(It.IsAny<AppUser>())).ReturnsAsync(true);
        _userManager.Setup(manager => manager.GeneratePasswordResetTokenAsync(It.IsAny<AppUser>()))
            .ReturnsAsync("code");

        // Act
        var result = await _service.ForgotPassword(forgot);

        // Assert
        Assert.IsType<Ok>(result);
        Assert.True(sentPasswordResetCode);
    }

    [Fact]
    public async Task ResetPassword_ReturnsValidationProblem_WhenUserDoesNotExist()
    {
        // Arrange
        var user = _fixture.Build<AppUser>().With(user => user.Email, "asdf@asdf.asdf").Create();
        _userManager.Setup(manager => manager.FindByEmailAsync(user.Email!)).ReturnsAsync(null as AppUser);
        _userManager.Setup(manager => manager.IsEmailConfirmedAsync(user)).ReturnsAsync(true);
        const string code = "pie";
        const string code64 = "cGll";
        var reset = new ResetPasswordRequest
        {
            Email = user.Email!,
            NewPassword = "hunter2",
            ResetCode = code64
        };
        var passwordReset = false;
        _userManager.Setup(manager => manager.ResetPasswordAsync(user, code, reset.NewPassword))
            .Callback(() => passwordReset = true).ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _service.ResetPassword(reset);

        // Assert
        Assert.IsType<ValidationProblem>(result);
        Assert.False(passwordReset);
    }

    [Fact]
    public async Task ResetPassword_ReturnsValidationProblem_WhenUserNotConfirmed()
    {
        // Arrange
        var user = _fixture.Build<AppUser>().With(user => user.Email, "asdf@asdf.asdf").Create();
        _userManager.Setup(manager => manager.FindByEmailAsync(user.Email!)).ReturnsAsync(user);
        _userManager.Setup(manager => manager.IsEmailConfirmedAsync(user)).ReturnsAsync(false);
        const string code = "pie";
        const string code64 = "cGll";
        var reset = new ResetPasswordRequest
        {
            Email = user.Email!,
            NewPassword = "hunter2",
            ResetCode = code64
        };
        var passwordReset = false;
        _userManager.Setup(manager => manager.ResetPasswordAsync(user, code, reset.NewPassword))
            .Callback(() => passwordReset = true).ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _service.ResetPassword(reset);

        // Assert
        Assert.IsType<ValidationProblem>(result);
        Assert.False(passwordReset);
    }

    [Fact]
    public async Task ResetPassword_ReturnsValidationProblem_WhenResetCodeNotBase64()
    {
        // Arrange
        var user = _fixture.Build<AppUser>().With(user => user.Email, "asdf@asdf.asdf").Create();
        _userManager.Setup(manager => manager.FindByEmailAsync(user.Email!)).ReturnsAsync(user);
        _userManager.Setup(manager => manager.IsEmailConfirmedAsync(user)).ReturnsAsync(true);
        const string code = "definitely not base64";
        var reset = new ResetPasswordRequest
        {
            Email = user.Email!,
            NewPassword = "hunter2",
            ResetCode = code
        };

        // Act
        var result = await _service.ResetPassword(reset);

        // Assert
        Assert.IsType<ValidationProblem>(result);
    }

    [Fact]
    public async Task ResetPassword_ReturnsValidationProblem_WhenResetCodeIncorrect()
    {
        // Arrange
        var user = _fixture.Build<AppUser>().With(user => user.Email, "asdf@asdf.asdf").Create();
        _userManager.Setup(manager => manager.FindByEmailAsync(user.Email!)).ReturnsAsync(null as AppUser);
        _userManager.Setup(manager => manager.IsEmailConfirmedAsync(user)).ReturnsAsync(true);
        const string code = "pie";
        const string code64 = "cGll";
        var reset = new ResetPasswordRequest
        {
            Email = user.Email!,
            NewPassword = "hunter2",
            ResetCode = code64
        };
        var passwordReset = false;
        _userManager.Setup(manager => manager.ResetPasswordAsync(user, code, reset.NewPassword))
            .Callback(() => passwordReset = true).ReturnsAsync(IdentityResult.Failed(new IdentityError
            {
                Code = "red",
                Description = "error"
            }));

        // Act
        var result = await _service.ResetPassword(reset);

        // Assert
        Assert.IsType<ValidationProblem>(result);
        Assert.False(passwordReset);
    }

    [Fact]
    public async Task ResetPassword_ReturnsOkAndResetsPassword_WhenResetSuccessful()
    {
        // Arrange
        var user = _fixture.Build<AppUser>().With(user => user.Email, "asdf@asdf.asdf").Create();
        _userManager.Setup(manager => manager.FindByEmailAsync(user.Email!)).ReturnsAsync(user);
        _userManager.Setup(manager => manager.IsEmailConfirmedAsync(user)).ReturnsAsync(true);
        const string code = "pie";
        const string code64 = "cGll";
        var reset = new ResetPasswordRequest
        {
            Email = user.Email!,
            NewPassword = "hunter2",
            ResetCode = code64
        };
        var passwordReset = false;
        _userManager.Setup(manager => manager.ResetPasswordAsync(user, code, reset.NewPassword))
            .Callback(() => passwordReset = true).ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _service.ResetPassword(reset);

        // Assert
        Assert.IsType<Ok>(result);
        Assert.True(passwordReset);
    }

    [Fact]
    public async Task Manage2Fa_ReturnsNotFound_WhenUserNotFound()
    {
        // Arrange
        var user = _fixture.Create<AppUser>();
        _userManager.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(null as AppUser);
        var request = new TwoFactorRequest
        {
            Enable = true,
            TwoFactorCode = "code"
        };

        // Act
        var result = await _service.Manage2Fa(request, new ClaimsPrincipal());

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task Manage2Fa_ReturnsValidationProblem_WhenTwoFactorEnableAndResetSharedKeyRequested()
    {
        // Arrange
        var user = _fixture.Create<AppUser>();
        _userManager.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        var request = new TwoFactorRequest
        {
            Enable = true,
            TwoFactorCode = "code",
            ResetSharedKey = true
        };

        // Act
        var result = await _service.Manage2Fa(request, new ClaimsPrincipal());

        // Assert
        Assert.IsType<ValidationProblem>(result);
    }

    [Fact]
    public async Task Manage2Fa_ReturnsValidationProblem_WhenTwoFactorEnableAndTwoFactorCodeEmpty()
    {
        // Arrange
        var user = _fixture.Create<AppUser>();
        _userManager.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        var request = new TwoFactorRequest
        {
            Enable = true
        };

        // Act
        var result = await _service.Manage2Fa(request, new ClaimsPrincipal());

        // Assert
        Assert.IsType<ValidationProblem>(result);
    }

    [Fact]
    public async Task Manage2Fa_ReturnsValidationProblem_WhenTwoFactorEnableAndTwoFactorCodeInvalid()
    {
        // Arrange
        var user = _fixture.Create<AppUser>();
        _userManager.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        var request = new TwoFactorRequest
        {
            Enable = true,
            TwoFactorCode = "code"
        };
        _userManager
            .Setup(manager => manager.VerifyTwoFactorTokenAsync(user, It.IsAny<string>(), request.TwoFactorCode))
            .ReturnsAsync(false);

        // Act
        var result = await _service.Manage2Fa(request, new ClaimsPrincipal());

        // Assert
        Assert.IsType<ValidationProblem>(result);
    }

    [Fact]
    public async Task Manage2Fa_ReturnsOkAndEnablesTwoFactor_WhenTwoFactorEnableSuccessful()
    {
        // Arrange
        var user = _fixture.Create<AppUser>();
        _userManager.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        var request = new TwoFactorRequest
        {
            Enable = true,
            TwoFactorCode = "code"
        };
        _userManager.Setup(manager => manager.CountRecoveryCodesAsync(user)).ReturnsAsync(10);
        _userManager.Setup(manager => manager.GetAuthenticatorKeyAsync(user)).ReturnsAsync("not null or empty");
        _userManager
            .Setup(manager => manager.VerifyTwoFactorTokenAsync(user, It.IsAny<string>(), request.TwoFactorCode))
            .ReturnsAsync(true);
        var twoFactorEnabled = false;
        _userManager.Setup(manager => manager.SetTwoFactorEnabledAsync(user, true))
            .Callback(() => twoFactorEnabled = true);

        // Act
        var result = await _service.Manage2Fa(request, new ClaimsPrincipal());

        // Assert
        Assert.IsType<Ok<TwoFactorResponse>>(result);
        Assert.True(twoFactorEnabled);
    }

    [Fact]
    public async Task Manage2Fa_ReturnsOkAndDisablesTwoFactor_WhenTwoFactorDisableSuccessful()
    {
        // Arrange
        var user = _fixture.Create<AppUser>();
        _userManager.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        var request = new TwoFactorRequest
        {
            Enable = false
        };
        _userManager.Setup(manager => manager.CountRecoveryCodesAsync(user)).ReturnsAsync(10);
        _userManager.Setup(manager => manager.GetAuthenticatorKeyAsync(user)).ReturnsAsync("not null or empty");
        var twoFactorDisabled = false;
        _userManager.Setup(manager => manager.SetTwoFactorEnabledAsync(user, false))
            .Callback(() => twoFactorDisabled = true);

        // Act
        var result = await _service.Manage2Fa(request, new ClaimsPrincipal());

        // Assert
        Assert.IsType<Ok<TwoFactorResponse>>(result);
        Assert.True(twoFactorDisabled);
    }

    [Fact]
    public async Task Manage2Fa_ReturnsOkAndResetsRecoveryCodes_WhenNoRecoveryCodesLeft()
    {
        // Arrange
        var user = _fixture.Create<AppUser>();
        _userManager.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        var request = new TwoFactorRequest
        {
            Enable = true,
            TwoFactorCode = "code"
        };
        _userManager.Setup(manager => manager.CountRecoveryCodesAsync(user)).ReturnsAsync(0);
        var codesGenerated = false;
        _userManager.Setup(manager => manager.GenerateNewTwoFactorRecoveryCodesAsync(user, It.IsAny<int>()))
            .Callback(() => codesGenerated = true);
        _userManager.Setup(manager => manager.GetAuthenticatorKeyAsync(user)).ReturnsAsync("not null or empty");
        _userManager
            .Setup(manager => manager.VerifyTwoFactorTokenAsync(user, It.IsAny<string>(), request.TwoFactorCode))
            .ReturnsAsync(true);
        var twoFactorEnabled = false;
        _userManager.Setup(manager => manager.SetTwoFactorEnabledAsync(user, true))
            .Callback(() => twoFactorEnabled = true);

        // Act
        var result = await _service.Manage2Fa(request, new ClaimsPrincipal());

        // Assert
        Assert.IsType<Ok<TwoFactorResponse>>(result);
        Assert.True(twoFactorEnabled);
        Assert.True(codesGenerated);
    }

    [Fact]
    public async Task GetInfo_ReturnsNotFound_WhenUserNotLoggedIn()
    {
        // Arrange
        _userManager.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(null as AppUser);

        // Act
        var result = await _service.GetInfo(new ClaimsPrincipal());

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task GetInfo_ReturnsOk_WhenUserLoggedIn()
    {
        // Arrange
        var user = _fixture.Create<AppUser>();
        _userManager.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        _userManager.Setup(manager => manager.GetEmailAsync(user)).ReturnsAsync(user.Email);

        // Act
        var result = await _service.GetInfo(new ClaimsPrincipal());

        // Assert
        Assert.IsType<Ok<InfoResponse>>(result);
    }

    [Fact]
    public async Task PostInfo_ReturnsNotFound_WhenUserNotLoggedIn()
    {
        var request = new InfoRequest
        {
            NewEmail = "asdf@asdf.asdf"
        };
        _userManager.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(null as AppUser);


        // Act
        var result = await _service.PostInfo(request, new DefaultHttpContext());

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Theory]
    [InlineData("the")]
    [InlineData("a@")]
    [InlineData("@qwerty")]
    public async Task PostInfo_ReturnsValidationProblem_WhenEmailNotValid(string email)
    {
        var request = new InfoRequest
        {
            NewEmail = email
        };
        var user = _fixture.Create<AppUser>();
        _userManager.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        _userManager.Setup(manager => manager.GetEmailAsync(user)).ReturnsAsync(user.Email);
        _userManager.Setup(manager => manager.GenerateChangeEmailTokenAsync(user, request.NewEmail))
            .ReturnsAsync("code");
        var emailSent = false;
        var passwordChanged = false;
        _emailSender
            .Setup(sender =>
                sender.SendConfirmationLinkAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>())).Callback(
                () => emailSent = true);
        _userManager.Setup(manager => manager.ChangePasswordAsync(user, request.OldPassword!, request.NewPassword!))
            .Callback(() => passwordChanged = true).ReturnsAsync(IdentityResult.Success);
        const string response = "Nothing really matters";
        _linkGenerator.Setup(m => m.GetUriByAddress(It.IsAny<HttpContext>(), It.IsAny<RouteValuesAddress>(),
                It.IsAny<RouteValueDictionary>(), default, default, default, default, default, default))
            .Returns(response);


        // Act
        var result = await _service.PostInfo(request, new DefaultHttpContext());

        // Assert
        Assert.IsType<ValidationProblem>(result);
        Assert.False(emailSent);
        Assert.False(passwordChanged);
    }

    [Fact]
    public async Task PostInfo_ReturnsValidationProblem_WhenNewPasswordProvidedWithoutOldPassword()
    {
        var request = new InfoRequest
        {
            NewPassword = "asdfsadfasdf"
        };
        var user = _fixture.Create<AppUser>();
        _userManager.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        _userManager.Setup(manager => manager.GetEmailAsync(user)).ReturnsAsync(user.Email);
        _userManager.Setup(manager => manager.GenerateChangeEmailTokenAsync(user, request.NewEmail))
            .ReturnsAsync("code");
        var emailSent = false;
        var passwordChanged = false;
        _emailSender
            .Setup(sender =>
                sender.SendConfirmationLinkAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>())).Callback(
                () => emailSent = true);
        _userManager.Setup(manager => manager.ChangePasswordAsync(user, request.OldPassword!, request.NewPassword!))
            .Callback(() => passwordChanged = true).ReturnsAsync(IdentityResult.Success);
        const string response = "Nothing really matters";
        _linkGenerator.Setup(m => m.GetUriByAddress(It.IsAny<HttpContext>(), It.IsAny<RouteValuesAddress>(),
                It.IsAny<RouteValueDictionary>(), default, default, default, default, default, default))
            .Returns(response);


        // Act
        var result = await _service.PostInfo(request, new DefaultHttpContext());

        // Assert
        Assert.IsType<ValidationProblem>(result);
        Assert.False(emailSent);
        Assert.False(passwordChanged);
    }


    [Fact]
    public async Task PostInfo_ReturnsValidationProblem_WhenChangingPasswordsFailed()
    {
        var request = new InfoRequest
        {
            NewPassword = "asdfsadfasdf",
            OldPassword = "lskdfjskldfj"
        };
        var user = _fixture.Create<AppUser>();
        _userManager.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        _userManager.Setup(manager => manager.GetEmailAsync(user)).ReturnsAsync(user.Email);
        _userManager.Setup(manager => manager.GenerateChangeEmailTokenAsync(user, request.NewEmail!))
            .ReturnsAsync("code");
        var emailSent = false;
        var passwordChanged = false;
        _emailSender
            .Setup(sender =>
                sender.SendConfirmationLinkAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>())).Callback(
                () => emailSent = true);
        _userManager.Setup(manager => manager.ChangePasswordAsync(user, request.OldPassword!, request.NewPassword!))
            .Callback(() => passwordChanged = false).ReturnsAsync(IdentityResult.Failed(new IdentityError
            {
                Code = "red",
                Description = "error"
            }));
        const string response = "Nothing really matters";
        _linkGenerator.Setup(m => m.GetUriByAddress(It.IsAny<HttpContext>(), It.IsAny<RouteValuesAddress>(),
                It.IsAny<RouteValueDictionary>(), default, default, default, default, default, default))
            .Returns(response);


        // Act
        var result = await _service.PostInfo(request, new DefaultHttpContext());

        // Assert
        Assert.IsType<ValidationProblem>(result);
        Assert.False(emailSent);
        Assert.False(passwordChanged);
    }

    [Fact]
    public async Task PostInfo_ReturnsOk_WhenChangingPasswordsSucceeded()
    {
        var request = new InfoRequest
        {
            NewPassword = "asdfsdfasdf",
            OldPassword = "lskdjflskjk"
        };
        var user = _fixture.Create<AppUser>();
        _userManager.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        _userManager.Setup(manager => manager.GetEmailAsync(user)).ReturnsAsync(user.Email);
        _userManager.Setup(manager => manager.GenerateChangeEmailTokenAsync(user, request.NewEmail!))
            .ReturnsAsync("code");
        var emailSent = false;
        var passwordChanged = false;
        _emailSender
            .Setup(sender =>
                sender.SendConfirmationLinkAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>())).Callback(
                () => emailSent = true);
        _userManager.Setup(manager => manager.ChangePasswordAsync(user, request.OldPassword!, request.NewPassword!))
            .Callback(() => passwordChanged = true).ReturnsAsync(IdentityResult.Success);
        const string response = "Nothing really matters";
        _linkGenerator.Setup(m => m.GetUriByAddress(It.IsAny<HttpContext>(), It.IsAny<RouteValuesAddress>(),
                It.IsAny<RouteValueDictionary>(), default, default, default, default, default, default))
            .Returns(response);


        // Act
        var result = await _service.PostInfo(request, new DefaultHttpContext());

        // Assert
        Assert.IsType<Ok<InfoResponse>>(result);
        Assert.False(emailSent);
        Assert.True(passwordChanged);
    }

    [Fact]
    public async Task PostInfo_ReturnsOk_WhenChangingEmailSucceeded()
    {
        var request = new InfoRequest
        {
            NewEmail = "asdf@asdf.asdf"
        };
        var user = _fixture.Create<AppUser>();
        _userManager.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        _userManager.Setup(manager => manager.GetEmailAsync(user)).ReturnsAsync(user.Email);
        _userManager.Setup(manager => manager.GenerateChangeEmailTokenAsync(user, request.NewEmail))
            .ReturnsAsync("code");
        var emailSent = false;
        var passwordChanged = false;
        _emailSender
            .Setup(sender =>
                sender.SendConfirmationLinkAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>())).Callback(
                () => emailSent = true);
        _userManager.Setup(manager => manager.ChangePasswordAsync(user, request.OldPassword!, request.NewPassword!))
            .Callback(() => passwordChanged = true).ReturnsAsync(IdentityResult.Success);
        const string response = "Nothing really matters";
        _linkGenerator.Setup(m => m.GetUriByAddress(It.IsAny<HttpContext>(), It.IsAny<RouteValuesAddress>(),
                It.IsAny<RouteValueDictionary>(), default, default, default, default, default, default))
            .Returns(response);


        // Act
        var result = await _service.PostInfo(request, new DefaultHttpContext());

        // Assert
        Assert.IsType<Ok<InfoResponse>>(result);
        Assert.True(emailSent);
        Assert.False(passwordChanged);
    }

    [Fact]
    public async Task SigninVk_ReturnsUnauthorized_WhenVkRespondedNotOk()
    {
        // Arrange
        var request = _fixture.Create<OauthCodeRequest>();
        var url1 = new UriBuilder(_configuration.GetConnectionString("VkAccessTokenUri")!);
        var query = HttpUtility.ParseQueryString(url1.Query);
        var vkConfig = _configuration.GetSection("Authentication:VK");
        query.Add("client_id", vkConfig["ClientId"]!);
        query.Add("client_secret", vkConfig["ClientSecret"]!);
        query.Add("redirect_uri", request.RedirectUri);
        query.Add("code", request.Code);
        url1.Query = query.ToString();
        var messageHandler = new Mock<HttpMessageHandler>();
        messageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(message => message.RequestUri == url1.Uri),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest));
        var client = new HttpClient(messageHandler.Object);
        _clientFactory.Setup(factory => factory.CreateClient(Options.DefaultName)).Returns(client);

        // Act
        var result = await _service.SigninVk(request);

        // Assert
        Assert.IsType<UnauthorizedHttpResult>(result);
    }

    [Fact]
    public async Task SigninVk_ReturnsBadRequest_WhenVkGaveIncorrectResponse()
    {
        // Arrange
        var request = _fixture.Create<OauthCodeRequest>();
        var url1 = new UriBuilder(_configuration.GetConnectionString("VkAccessTokenUri")!);
        var query = HttpUtility.ParseQueryString(url1.Query);
        var vkConfig = _configuration.GetSection("Authentication:VK");
        query.Add("client_id", vkConfig["ClientId"]!);
        query.Add("client_secret", vkConfig["ClientSecret"]!);
        query.Add("redirect_uri", request.RedirectUri);
        query.Add("code", request.Code);
        url1.Query = query.ToString();
        var messageHandler = new Mock<HttpMessageHandler>();
        messageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(message => message.RequestUri == url1.Uri),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"error_description\":\"asdfasdfasdf\"}", Encoding.UTF8)
            });
        var client = new HttpClient(messageHandler.Object);
        _clientFactory.Setup(factory => factory.CreateClient(Options.DefaultName)).Returns(client);

        // Act
        var result = await _service.SigninVk(request);

        // Assert
        Assert.IsType<BadRequest<string>>(result);
    }


    [Fact]
    public async Task SigninVk_ReturnsUnauthorized_WhenEmailNewAndCouldNotGetProfileInfo()
    {
        // Arrange
        var request = _fixture.Create<OauthCodeRequest>();
        var url1 = new UriBuilder(_configuration.GetConnectionString("VkAccessTokenUri")!);
        var query = HttpUtility.ParseQueryString(url1.Query);
        var vkConfig = _configuration.GetSection("Authentication:VK");
        query.Add("client_id", vkConfig["ClientId"]!);
        query.Add("client_secret", vkConfig["ClientSecret"]!);
        query.Add("redirect_uri", request.RedirectUri);
        query.Add("code", request.Code);
        url1.Query = query.ToString();
        var messageHandler = new Mock<HttpMessageHandler>();
        messageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(message => message.RequestUri == url1.Uri),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    "{\"access_token\":\"asdfasdfasdf\",\"expires_in\":0,\"user_id\":0,\"email\":\"asdf@asdf.asdf\"}",
                    Encoding.UTF8)
            });
        var client = new HttpClient(messageHandler.Object);
        _clientFactory.Setup(factory => factory.CreateClient(Options.DefaultName)).Returns(client);
        _userManager.Setup(manager => manager.FindByEmailAsync("asdf@asdf.asdf")).ReturnsAsync(null as AppUser);

        var url2 = new UriBuilder(_configuration.GetConnectionString("VkGetProfileInfoMethodUri")!);
        query = HttpUtility.ParseQueryString(url2.Query);
        query.Add("access_token", "asdfasdfasdf");
        query.Add("v", _configuration["VkApiVersion"]);
        url2.Query = query.ToString();

        messageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(message => message.RequestUri == url2.Uri),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest));
        // Act
        var result = await _service.SigninVk(request);

        // Assert
        Assert.IsType<UnauthorizedHttpResult>(result);
    }


    [Fact]
    public async Task SigninVk_ReturnsUnauthorized_WhenEmailNewAndVkGaveInvalidProfileInfo()
    {
        // Arrange
        var request = _fixture.Create<OauthCodeRequest>();
        var url1 = new UriBuilder(_configuration.GetConnectionString("VkAccessTokenUri")!);
        var query = HttpUtility.ParseQueryString(url1.Query);
        var vkConfig = _configuration.GetSection("Authentication:VK");
        query.Add("client_id", vkConfig["ClientId"]!);
        query.Add("client_secret", vkConfig["ClientSecret"]!);
        query.Add("redirect_uri", request.RedirectUri);
        query.Add("code", request.Code);
        url1.Query = query.ToString();
        var messageHandler = new Mock<HttpMessageHandler>();
        messageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(message => message.RequestUri == url1.Uri),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    "{\"access_token\":\"asdfasdfasdf\",\"expires_in\":0,\"user_id\":0,\"email\":\"asdf@asdf.asdf\"}",
                    Encoding.UTF8)
            });
        var client = new HttpClient(messageHandler.Object);
        _clientFactory.Setup(factory => factory.CreateClient(Options.DefaultName)).Returns(client);
        _userManager.Setup(manager => manager.FindByEmailAsync("asdf@asdf.asdf")).ReturnsAsync(null as AppUser);

        var url2 = new UriBuilder(_configuration.GetConnectionString("VkGetProfileInfoMethodUri")!);
        query = HttpUtility.ParseQueryString(url2.Query);
        query.Add("access_token", "asdfasdfasdf");
        query.Add("v", _configuration["VkApiVersion"]);
        url2.Query = query.ToString();

        messageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(message => message.RequestUri == url2.Uri),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{}", Encoding.UTF8)
            });

        // Act
        var result = await _service.SigninVk(request);

        // Assert
        Assert.IsType<UnauthorizedHttpResult>(result);
    }


    [Fact]
    public async Task SigninVk_ReturnsEmptyAndRegistersUser_WhenEmailNew()
    {
        // Arrange
        var request = _fixture.Create<OauthCodeRequest>();
        var url1 = new UriBuilder(_configuration.GetConnectionString("VkAccessTokenUri")!);
        var query = HttpUtility.ParseQueryString(url1.Query);
        var vkConfig = _configuration.GetSection("Authentication:VK");
        query.Add("client_id", vkConfig["ClientId"]!);
        query.Add("client_secret", vkConfig["ClientSecret"]!);
        query.Add("redirect_uri", request.RedirectUri);
        query.Add("code", request.Code);
        url1.Query = query.ToString();
        var messageHandler = new Mock<HttpMessageHandler>();
        messageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(message => message.RequestUri == url1.Uri),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    "{\"access_token\":\"asdfasdfasdf\",\"expires_in\":0,\"user_id\":0,\"email\":\"asdf@asdf.asdf\"}",
                    Encoding.UTF8)
            });
        var client = new HttpClient(messageHandler.Object);
        _clientFactory.Setup(factory => factory.CreateClient(Options.DefaultName)).Returns(client);
        _userManager.Setup(manager => manager.FindByEmailAsync("asdf@asdf.asdf")).ReturnsAsync(null as AppUser);

        var url2 = new UriBuilder(_configuration.GetConnectionString("VkGetProfileInfoMethodUri")!);
        query = HttpUtility.ParseQueryString(url2.Query);
        query.Add("access_token", "asdfasdfasdf");
        query.Add("v", _configuration["VkApiVersion"]);
        url2.Query = query.ToString();

        messageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(message => message.RequestUri == url2.Uri),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    "{\"response\":{\"photo_200\":\"\",\"first_name\":\"Asdf\",\"last_name\":\"Asdfasdf\",\"sex\":0}}",
                    Encoding.UTF8)
            });
        AppUser? createdUser = null;
        var addedRole = false;
        _userManager.Setup(manager => manager.CreateAsync(It.IsAny<AppUser>())).Callback((AppUser user) =>
        {
            createdUser = user;
        }).ReturnsAsync(IdentityResult.Success);
        _userManager.Setup(manager => manager.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>())).Callback(() =>
        {
            addedRole = true;
        });
        var signedIn = false;
        _signInManager.Setup(manager => manager.SignInAsync(It.IsAny<AppUser>(), false, It.IsAny<string?>()))
            .Callback(() => signedIn = true);

        // Act
        var result = await _service.SigninVk(request);

        // Assert
        Assert.IsType<EmptyHttpResult>(result);
        Assert.NotNull(createdUser);
        Assert.Equal(Gender.Other, createdUser!.Gender);
        Assert.Equal("Asdf", createdUser!.FirstName);
        Assert.Equal("Asdfasdf", createdUser!.LastName);
        Assert.True(addedRole);
        Assert.True(signedIn);
    }

    [Theory]
    [InlineData(1, Gender.Female)]
    [InlineData(2, Gender.Male)]
    [InlineData(0, Gender.Other)]
    public async Task SigninVk_ReturnsEmptyAndRegistersUserWithCorrectGender_WhenEmailNew(int sex, Gender gender)
    {
        // Arrange
        var request = _fixture.Create<OauthCodeRequest>();
        var url1 = new UriBuilder(_configuration.GetConnectionString("VkAccessTokenUri")!);
        var query = HttpUtility.ParseQueryString(url1.Query);
        var vkConfig = _configuration.GetSection("Authentication:VK");
        query.Add("client_id", vkConfig["ClientId"]!);
        query.Add("client_secret", vkConfig["ClientSecret"]!);
        query.Add("redirect_uri", request.RedirectUri);
        query.Add("code", request.Code);
        url1.Query = query.ToString();
        var messageHandler = new Mock<HttpMessageHandler>();
        messageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(message => message.RequestUri == url1.Uri),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    "{\"access_token\":\"asdfasdfasdf\",\"expires_in\":0,\"user_id\":0,\"email\":\"asdf@asdf.asdf\"}",
                    Encoding.UTF8)
            });
        var client = new HttpClient(messageHandler.Object);
        _clientFactory.Setup(factory => factory.CreateClient(Options.DefaultName)).Returns(client);
        _userManager.Setup(manager => manager.FindByEmailAsync("asdf@asdf.asdf")).ReturnsAsync(null as AppUser);

        var url2 = new UriBuilder(_configuration.GetConnectionString("VkGetProfileInfoMethodUri")!);
        query = HttpUtility.ParseQueryString(url2.Query);
        query.Add("access_token", "asdfasdfasdf");
        query.Add("v", _configuration["VkApiVersion"]);
        url2.Query = query.ToString();

        messageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(message => message.RequestUri == url2.Uri),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    "{\"response\":{\"photo_200\":\"\",\"first_name\":\"Asdf\",\"last_name\":\"Asdfasdf\",\"sex\":" +
                    sex + "}}", Encoding.UTF8)
            });
        AppUser? createdUser = null;
        var addedRole = false;
        _userManager.Setup(manager => manager.CreateAsync(It.IsAny<AppUser>())).Callback((AppUser user) =>
        {
            createdUser = user;
        }).ReturnsAsync(IdentityResult.Success);
        _userManager.Setup(manager => manager.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>())).Callback(() =>
        {
            addedRole = true;
        });
        var signedIn = false;
        _signInManager.Setup(manager => manager.SignInAsync(It.IsAny<AppUser>(), false, It.IsAny<string?>()))
            .Callback(() => signedIn = true);

        // Act
        var result = await _service.SigninVk(request);

        // Assert
        Assert.IsType<EmptyHttpResult>(result);
        Assert.NotNull(createdUser);
        Assert.Equal(gender, createdUser!.Gender);
        Assert.Equal("Asdf", createdUser!.FirstName);
        Assert.Equal("Asdfasdf", createdUser!.LastName);
        Assert.True(addedRole);
        Assert.True(signedIn);
    }

    [Fact]
    public async Task SigninVk_ReturnsEmptyAndLogsUserIn_WhenEmailExists()
    {
        // Arrange
        var request = _fixture.Create<OauthCodeRequest>();
        var url1 = new UriBuilder(_configuration.GetConnectionString("VkAccessTokenUri")!);
        var query = HttpUtility.ParseQueryString(url1.Query);
        var vkConfig = _configuration.GetSection("Authentication:VK");
        query.Add("client_id", vkConfig["ClientId"]!);
        query.Add("client_secret", vkConfig["ClientSecret"]!);
        query.Add("redirect_uri", request.RedirectUri);
        query.Add("code", request.Code);
        url1.Query = query.ToString();
        var messageHandler = new Mock<HttpMessageHandler>();
        messageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(message => message.RequestUri == url1.Uri),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    "{\"access_token\":\"asdfasdfasdf\",\"expires_in\":0,\"user_id\":0,\"email\":\"asdf@asdf.asdf\"}",
                    Encoding.UTF8)
            });
        var client = new HttpClient(messageHandler.Object);
        _clientFactory.Setup(factory => factory.CreateClient(Options.DefaultName)).Returns(client);
        var user = _fixture.Create<AppUser>();
        user.Email = "asdf@asdf.asdf";
        _userManager.Setup(manager => manager.FindByEmailAsync(user.Email)).ReturnsAsync(user);

        var signedIn = false;
        _signInManager.Setup(manager => manager.SignInAsync(It.IsAny<AppUser>(), false, It.IsAny<string?>()))
            .Callback(() => signedIn = true);

        // Act
        var result = await _service.SigninVk(request);

        // Assert
        Assert.IsType<EmptyHttpResult>(result);
        Assert.True(signedIn);
    }
}
