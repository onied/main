using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Web;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Users.Data.Entities;
using Users.Data.Enums;
using Users.Dtos.Users.Request;
using Users.Dtos.Users.Response;
using Users.Dtos.VkOauth.Request;
using Users.Dtos.VkOauth.Response;
using Users.Factories;
using Users.Services.UserCreatedProducer;

namespace Users.Services.UsersService;

public class UsersService(
    UserManager<AppUser> userManager,
    IUserStore<AppUser> userStore,
    LinkGenerator linkGenerator,
    IEmailSender<AppUser> emailSender,
    IUserCreatedProducer userCreatedProducer,
    SignInManager<AppUser> signInManager,
    IOptionsMonitor<BearerTokenOptions> bearerTokenOptions,
    TimeProvider timeProvider,
    IHttpClientFactory clientFactory,
    IConfiguration configuration) : IUsersService
{
    private static readonly EmailAddressAttribute EmailAddressAttribute = new();

    public async Task<IResult> Register(RegisterUserRequest registration, HttpContext context)
    {
        if (!userManager.SupportsUserEmail)
            throw new NotSupportedException($"{nameof(Register)} requires a user store with email support.");

        var emailStore = (IUserEmailStore<AppUser>)userStore;
        var email = registration.Email;

        if (string.IsNullOrEmpty(email) || !EmailAddressAttribute.IsValid(email))
            return ValidationProblemFactory.CreateValidationProblem(
                IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(email)));

        var user = new AppUser();
        await userStore.SetUserNameAsync(user, email, CancellationToken.None);
        await emailStore.SetEmailAsync(user, email, CancellationToken.None);
        user.Gender = registration.Gender;
        user.FirstName = registration.FirstName;
        user.LastName = registration.LastName;
        var result = await userManager.CreateAsync(user, registration.Password);

        if (!result.Succeeded) return ValidationProblemFactory.CreateValidationProblem(result);
        await userManager.AddToRoleAsync(user, "Student");

        await userCreatedProducer.PublishAsync(user);
        await SendConfirmationEmailAsync(user, context, email);
        return TypedResults.Ok();
    }

    public async Task<IResult> Login(LoginRequest login)
    {
        const bool useCookieScheme = false;
        const bool isPersistent = false;
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

    public async Task<IResult> Refresh(RefreshRequest refreshRequest)
    {
        var refreshTokenProtector = bearerTokenOptions.Get(IdentityConstants.BearerScheme).RefreshTokenProtector;
        var refreshTicket = refreshTokenProtector.Unprotect(refreshRequest.RefreshToken);

        // Reject the /refresh attempt with a 401 if the token expired or the security stamp validation fails
        if (refreshTicket?.Properties?.ExpiresUtc is not { } expiresUtc ||
            timeProvider.GetUtcNow() >= expiresUtc ||
            await signInManager.ValidateSecurityStampAsync(refreshTicket.Principal) is not { } user)
            return TypedResults.Challenge();

        var newPrincipal = await signInManager.CreateUserPrincipalAsync(user);
        return TypedResults.SignIn(newPrincipal, authenticationScheme: IdentityConstants.BearerScheme);
    }

    public async Task<IResult> ConfirmEmail(string userId, string code, string? changedEmail)
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

    public async Task<IResult> ResendConfirmationEmail(ResendConfirmationEmailRequest resendRequest,
        HttpContext context)
    {
        if (await userManager.FindByEmailAsync(resendRequest.Email) is not { } user) return TypedResults.Ok();

        await SendConfirmationEmailAsync(user, context, resendRequest.Email);
        return TypedResults.Ok();
    }

    public async Task<IResult> ForgotPassword(ForgotPasswordRequest resetRequest)
    {
        var user = await userManager.FindByEmailAsync(resetRequest.Email);

        if (user is not null && await userManager.IsEmailConfirmedAsync(user))
        {
            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            await emailSender.SendPasswordResetCodeAsync(user, resetRequest.Email, HtmlEncoder.Default.Encode(code));
        }

        // Don't reveal that the user does not exist or is not confirmed, so don't return a 200 if we would have
        // returned a 400 for an invalid code given a valid user email.
        return TypedResults.Ok();
    }

    public async Task<IResult> ResetPassword(ResetPasswordRequest resetRequest)
    {
        var user = await userManager.FindByEmailAsync(resetRequest.Email);

        if (user is null || !await userManager.IsEmailConfirmedAsync(user))
            // Don't reveal that the user does not exist or is not confirmed, so don't return a 200 if we would have
            // returned a 400 for an invalid code given a valid user email.
            return ValidationProblemFactory.CreateValidationProblem(
                IdentityResult.Failed(userManager.ErrorDescriber.InvalidToken()));

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

        if (!result.Succeeded) return ValidationProblemFactory.CreateValidationProblem(result);

        return TypedResults.Ok();
    }

    public async Task<IResult> Manage2Fa(TwoFactorRequest tfaRequest, ClaimsPrincipal claimsPrincipal)
    {
        if (await userManager.GetUserAsync(claimsPrincipal) is not { } user)
            return TypedResults.NotFound();

        if (tfaRequest.Enable == true)
        {
            if (tfaRequest.ResetSharedKey)
                return ValidationProblemFactory.CreateValidationProblem("CannotResetSharedKeyAndEnable",
                    "Resetting the 2fa shared key must disable 2fa until a 2fa token based on the new shared key is validated.");
            if (string.IsNullOrEmpty(tfaRequest.TwoFactorCode))
                return ValidationProblemFactory.CreateValidationProblem("RequiresTwoFactor",
                    "No 2fa token was provided by the request. A valid 2fa token is required to enable 2fa.");
            if (!await userManager.VerifyTwoFactorTokenAsync(user,
                    userManager.Options.Tokens.AuthenticatorTokenProvider, tfaRequest.TwoFactorCode))
                return ValidationProblemFactory.CreateValidationProblem("InvalidTwoFactorCode",
                    "The 2fa token provided by the request was invalid. A valid 2fa token is required to enable 2fa.");

            await userManager.SetTwoFactorEnabledAsync(user, true);
        }
        else if (tfaRequest.Enable == false || tfaRequest.ResetSharedKey)
        {
            await userManager.SetTwoFactorEnabledAsync(user, false);
        }

        if (tfaRequest.ResetSharedKey) await userManager.ResetAuthenticatorKeyAsync(user);

        string[]? recoveryCodes = null;
        if (tfaRequest.ResetRecoveryCodes ||
            (tfaRequest.Enable == true && await userManager.CountRecoveryCodesAsync(user) == 0))
        {
            var recoveryCodesEnumerable = await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            recoveryCodes = recoveryCodesEnumerable?.ToArray();
        }

        if (tfaRequest.ForgetMachine) await signInManager.ForgetTwoFactorClientAsync();

        var key = await userManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrEmpty(key))
        {
            await userManager.ResetAuthenticatorKeyAsync(user);
            key = await userManager.GetAuthenticatorKeyAsync(user);

            if (string.IsNullOrEmpty(key))
                throw new NotSupportedException("The user manager must produce an authenticator key after reset.");
        }

        return TypedResults.Ok(new TwoFactorResponse
        {
            SharedKey = key,
            RecoveryCodes = recoveryCodes,
            RecoveryCodesLeft = recoveryCodes?.Length ?? await userManager.CountRecoveryCodesAsync(user),
            IsTwoFactorEnabled = await userManager.GetTwoFactorEnabledAsync(user),
            IsMachineRemembered = await signInManager.IsTwoFactorClientRememberedAsync(user)
        });
    }

    public async Task<IResult> Get2FaInfo(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null) return TypedResults.NotFound();

        var response = new TwoFactorEnabledResponse(user.TwoFactorEnabled);
        return TypedResults.Ok(response);
    }

    public async Task<IResult> GetInfo(ClaimsPrincipal claimsPrincipal)
    {
        if (await userManager.GetUserAsync(claimsPrincipal) is not { } user)
            return TypedResults.NotFound();

        return TypedResults.Ok(new InfoResponse
        {
            Email = await userManager.GetEmailAsync(user) ??
                    throw new NotSupportedException("Users must have an email."),
            IsEmailConfirmed = await userManager.IsEmailConfirmedAsync(user)
        });
    }

    public async Task<IResult> PostInfo(InfoRequest infoRequest, HttpContext context)
    {
        if (await userManager.GetUserAsync(context.User) is not { } user)
            return TypedResults.NotFound();

        if (!string.IsNullOrEmpty(infoRequest.NewEmail) && !EmailAddressAttribute.IsValid(infoRequest.NewEmail))
            return ValidationProblemFactory.CreateValidationProblem(
                IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(infoRequest.NewEmail)));

        if (!string.IsNullOrEmpty(infoRequest.NewPassword))
        {
            if (string.IsNullOrEmpty(infoRequest.OldPassword))
                return ValidationProblemFactory.CreateValidationProblem("OldPasswordRequired",
                    "The old password is required to set a new password. If the old password is forgotten, use /resetPassword.");

            var changePasswordResult =
                await userManager.ChangePasswordAsync(user, infoRequest.OldPassword, infoRequest.NewPassword);
            if (!changePasswordResult.Succeeded)
                return ValidationProblemFactory.CreateValidationProblem(changePasswordResult);
        }

        if (!string.IsNullOrEmpty(infoRequest.NewEmail))
        {
            var email = await userManager.GetEmailAsync(user);

            if (email != infoRequest.NewEmail)
                await SendConfirmationEmailAsync(user, context, infoRequest.NewEmail, true);
        }

        return TypedResults.Ok(new InfoResponse
        {
            Email = await userManager.GetEmailAsync(user) ??
                    throw new NotSupportedException("Users must have an email."),
            IsEmailConfirmed = await userManager.IsEmailConfirmedAsync(user)
        });
    }

    public async Task<IResult> SigninVk(OauthCodeRequest oauthCodeRequest)
    {
        var client = clientFactory.CreateClient();
        var url = new UriBuilder(configuration.GetConnectionString("VkAccessTokenUri")!);
        var query = HttpUtility.ParseQueryString(url.Query);
        var vkConfig = configuration.GetSection("Authentication:VK");
        query.Add("client_id", vkConfig["ClientId"]!);
        query.Add("client_secret", vkConfig["ClientSecret"]!);
        query.Add("redirect_uri", oauthCodeRequest.RedirectUri);
        query.Add("code", oauthCodeRequest.Code);
        url.Query = query.ToString();
        var response = await client.GetAsync(url.ToString());
        if (!response.IsSuccessStatusCode)
            return Results.Unauthorized();
        var responseContent = await response.Content.ReadAsStreamAsync();
        var accessTokenResponse = await JsonSerializer.DeserializeAsync<VkAccessTokenResponse>(responseContent);
        if (accessTokenResponse?.Email == null)
            return Results.BadRequest(accessTokenResponse?.ErrorDescription);

        var user = await userManager.FindByEmailAsync(accessTokenResponse.Email);
        if (user == null)
        {
            url = new UriBuilder(configuration.GetConnectionString("VkGetProfileInfoMethodUri")!);
            query = HttpUtility.ParseQueryString(url.Query);
            query.Add("access_token", accessTokenResponse.AccessToken);
            query.Add("v", configuration["VkApiVersion"]);
            url.Query = query.ToString();
            response = await client.GetAsync(url.ToString());
            if (!response.IsSuccessStatusCode)
                return Results.Unauthorized();
            responseContent = await response.Content.ReadAsStreamAsync();
            var userInfoResponseWrapper =
                await JsonSerializer.DeserializeAsync<UserInfoResponseWrapper>(responseContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (userInfoResponseWrapper?.Response == null)
                return Results.Unauthorized();
            user = new AppUser();
            await userStore.SetUserNameAsync(user, accessTokenResponse.Email, CancellationToken.None);
            var emailStore = (IUserEmailStore<AppUser>)userStore;
            await emailStore.SetEmailAsync(user, accessTokenResponse.Email, CancellationToken.None);
            user.Gender = userInfoResponseWrapper.Response.Sex switch
            {
                1 => Gender.Female,
                2 => Gender.Male,
                _ => Gender.Other
            };
            user.FirstName = userInfoResponseWrapper.Response.FirstName;
            user.LastName = userInfoResponseWrapper.Response.LastName;
            user.Avatar = userInfoResponseWrapper.Response.Photo200;
            var result = await userManager.CreateAsync(user);

            if (!result.Succeeded) return Results.Unauthorized();
        }

        signInManager.AuthenticationScheme = IdentityConstants.BearerScheme;

        await signInManager.SignInAsync(user, false);
        await userCreatedProducer.PublishAsync(user);
        return Results.Empty;
    }

    private async Task SendConfirmationEmailAsync(AppUser user, HttpContext context, string email,
        bool isChange = false)
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

        var confirmEmailUrl = linkGenerator.GetUriByAction(context, nameof(ConfirmEmail), "Users", routeValues)
                              ?? throw new NotSupportedException(
                                  $"Could not find endpoint named '{nameof(ConfirmEmail)}'.");

        await emailSender.SendConfirmationLinkAsync(user, email, HtmlEncoder.Default.Encode(confirmEmailUrl));
    }
}
