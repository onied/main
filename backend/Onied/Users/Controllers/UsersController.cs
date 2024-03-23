using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Users.Dtos;

namespace Users.Controllers;

[ApiController]
[Route("/api/v1/[action]")]
public class UsersController : ControllerBase
{
    private static readonly EmailAddressAttribute EmailAddressAttribute = new();
    private readonly IOptionsMonitor<BearerTokenOptions> _bearerTokenOptions;
    private readonly IEmailSender<AppUser> _emailSender;
    private readonly LinkGenerator _linkGenerator;
    private readonly TimeProvider _timeProvider;

    public UsersController(IEmailSender<AppUser> emailSender, LinkGenerator linkGenerator,
        IOptionsMonitor<BearerTokenOptions> bearerTokenOptions, TimeProvider timeProvider)
    {
        _emailSender = emailSender;
        _linkGenerator = linkGenerator;
        _bearerTokenOptions = bearerTokenOptions;
        _timeProvider = timeProvider;
    }

    [HttpPost]
    public async Task<Results<Ok, ValidationProblem>> Register([FromBody] RegisterUserDto registration,
        [FromServices] UserManager<AppUser> userManager, [FromServices] IUserStore<AppUser> userStore)
    {
        if (!userManager.SupportsUserEmail)
            throw new NotSupportedException($"{nameof(Register)} requires a user store with email support.");

        var emailStore = (IUserEmailStore<AppUser>)userStore;
        var email = registration.Email;

        if (string.IsNullOrEmpty(email) || !EmailAddressAttribute.IsValid(email))
            return CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(email)));

        var user = new AppUser();
        await userStore.SetUserNameAsync(user, email, CancellationToken.None);
        await emailStore.SetEmailAsync(user, email, CancellationToken.None);
        user.Gender = registration.Gender;
        user.FirstName = registration.FirstName;
        user.LastName = registration.LastName;
        var result = await userManager.CreateAsync(user, registration.Password);

        if (!result.Succeeded) return CreateValidationProblem(result);

        await SendConfirmationEmailAsync(user, userManager, HttpContext, email);
        return TypedResults.Ok();
    }

    [HttpPost]
    public async Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>> Login(
        [FromBody] LoginRequest login, [FromQuery] bool? useCookies, [FromQuery] bool? useSessionCookies,
        [FromServices] SignInManager<AppUser> signInManager)
    {
        var useCookieScheme = useCookies == true || useSessionCookies == true;
        var isPersistent = useCookies == true && useSessionCookies != true;
        signInManager.AuthenticationScheme =
            useCookieScheme ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;

        var result =
            await signInManager.PasswordSignInAsync(login.Email, login.Password, isPersistent, true);

        if (result.RequiresTwoFactor)
        {
            if (!string.IsNullOrEmpty(login.TwoFactorCode))
                result = await signInManager.TwoFactorAuthenticatorSignInAsync(login.TwoFactorCode, isPersistent,
                    isPersistent);
            else if (!string.IsNullOrEmpty(login.TwoFactorRecoveryCode))
                result = await signInManager.TwoFactorRecoveryCodeSignInAsync(login.TwoFactorRecoveryCode);
        }

        if (!result.Succeeded)
            return TypedResults.Problem(result.ToString(), statusCode: StatusCodes.Status401Unauthorized);

        // The signInManager already produced the needed response in the form of a cookie or bearer token.
        return TypedResults.Empty;
    }

    [HttpPost]
    public async Task<Results<Ok<AccessTokenResponse>, UnauthorizedHttpResult, SignInHttpResult, ChallengeHttpResult>>
        Refresh
        ([FromBody] RefreshRequest refreshRequest, [FromServices] SignInManager<AppUser> signInManager)
    {
        var refreshTokenProtector = _bearerTokenOptions.Get(IdentityConstants.BearerScheme).RefreshTokenProtector;
        var refreshTicket = refreshTokenProtector.Unprotect(refreshRequest.RefreshToken);

        // Reject the /refresh attempt with a 401 if the token expired or the security stamp validation fails
        if (refreshTicket?.Properties?.ExpiresUtc is not { } expiresUtc ||
            _timeProvider.GetUtcNow() >= expiresUtc ||
            await signInManager.ValidateSecurityStampAsync(refreshTicket.Principal) is not { } user)
            return TypedResults.Challenge();

        var newPrincipal = await signInManager.CreateUserPrincipalAsync(user);
        return TypedResults.SignIn(newPrincipal, authenticationScheme: IdentityConstants.BearerScheme);
    }

    [HttpGet]
    public async Task<Results<ContentHttpResult, UnauthorizedHttpResult>> ConfirmEmail
    ([FromQuery] string userId, [FromQuery] string code, [FromQuery] string? changedEmail,
        [FromServices] UserManager<AppUser> userManager)
    {
        if (await userManager.FindByIdAsync(userId) is not { } user)
            // We could respond with a 404 instead of a 401 like Identity UI, but that feels like unnecessary information.
            return TypedResults.Unauthorized();

        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        }
        catch (FormatException)
        {
            return TypedResults.Unauthorized();
        }

        IdentityResult result;

        if (string.IsNullOrEmpty(changedEmail))
        {
            result = await userManager.ConfirmEmailAsync(user, code);
        }
        else
        {
            // As with Identity UI, email and user name are one and the same. So when we update the email,
            // we need to update the user name.
            result = await userManager.ChangeEmailAsync(user, changedEmail, code);

            if (result.Succeeded) result = await userManager.SetUserNameAsync(user, changedEmail);
        }

        if (!result.Succeeded) return TypedResults.Unauthorized();

        return TypedResults.Text("Thank you for confirming your email.");
    }

    [HttpPost]
    public async Task<Ok> ResendConfirmationEmail
        ([FromBody] ResendConfirmationEmailRequest resendRequest, [FromServices] UserManager<AppUser> userManager)
    {
        if (await userManager.FindByEmailAsync(resendRequest.Email) is not { } user) return TypedResults.Ok();

        await SendConfirmationEmailAsync(user, userManager, HttpContext, resendRequest.Email);
        return TypedResults.Ok();
    }

    [HttpPost]
    public async Task<Results<Ok, ValidationProblem>> ForgotPassword
        ([FromBody] ForgotPasswordRequest resetRequest, [FromServices] UserManager<AppUser> userManager)
    {
        var user = await userManager.FindByEmailAsync(resetRequest.Email);

        if (user is not null && await userManager.IsEmailConfirmedAsync(user))
        {
            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            await _emailSender.SendPasswordResetCodeAsync(user, resetRequest.Email, HtmlEncoder.Default.Encode(code));
        }

        // Don't reveal that the user does not exist or is not confirmed, so don't return a 200 if we would have
        // returned a 400 for an invalid code given a valid user email.
        return TypedResults.Ok();
    }

    [HttpPost]
    public async Task<Results<Ok, ValidationProblem>> ResetPassword
        ([FromBody] ResetPasswordRequest resetRequest, [FromServices] UserManager<AppUser> userManager)
    {
        var user = await userManager.FindByEmailAsync(resetRequest.Email);

        if (user is null || !await userManager.IsEmailConfirmedAsync(user))
            // Don't reveal that the user does not exist or is not confirmed, so don't return a 200 if we would have
            // returned a 400 for an invalid code given a valid user email.
            return CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidToken()));

        IdentityResult result;
        try
        {
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetRequest.ResetCode));
            result = await userManager.ResetPasswordAsync(user, code, resetRequest.NewPassword);
        }
        catch (FormatException)
        {
            result = IdentityResult.Failed(userManager.ErrorDescriber.InvalidToken());
        }

        if (!result.Succeeded) return CreateValidationProblem(result);

        return TypedResults.Ok();
    }

    private async Task SendConfirmationEmailAsync(AppUser user, UserManager<AppUser> userManager, HttpContext context,
        string email, bool isChange = false)
    {
        var code = isChange
            ? await userManager.GenerateChangeEmailTokenAsync(user, email)
            : await userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var userId = await userManager.GetUserIdAsync(user);
        var routeValues = new RouteValueDictionary
        {
            ["userId"] = userId,
            ["code"] = code
        };

        if (isChange)
            // This is validated by the /confirmEmail endpoint on change.
            routeValues.Add("changedEmail", email);

        var confirmEmailUrl = _linkGenerator.GetUriByAction(context, nameof(ConfirmEmail), "Users")
                              ?? throw new NotSupportedException(
                                  $"Could not find endpoint named '{nameof(ConfirmEmail)}'.");

        await _emailSender.SendConfirmationLinkAsync(user, email, HtmlEncoder.Default.Encode(confirmEmailUrl));
    }

    private static ValidationProblem CreateValidationProblem(IdentityResult result)
    {
        // We expect a single error code and description in the normal case.
        // This could be golfed with GroupBy and ToDictionary, but perf! :P
        Debug.Assert(!result.Succeeded);
        var errorDictionary = new Dictionary<string, string[]>(1);

        foreach (var error in result.Errors)
        {
            string[] newDescriptions;

            if (errorDictionary.TryGetValue(error.Code, out var descriptions))
            {
                newDescriptions = new string[descriptions.Length + 1];
                Array.Copy(descriptions, newDescriptions, descriptions.Length);
                newDescriptions[descriptions.Length] = error.Description;
            }
            else
            {
                newDescriptions = [error.Description];
            }

            errorDictionary[error.Code] = newDescriptions;
        }

        return TypedResults.ValidationProblem(errorDictionary);
    }
}
