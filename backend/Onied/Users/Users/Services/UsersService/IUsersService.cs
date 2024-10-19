using System.Security.Claims;
using Microsoft.AspNetCore.Identity.Data;
using Users.Dtos.Users.Request;
using Users.Dtos.VkOauth.Request;

namespace Users.Services.UsersService;

public interface IUsersService
{
    public Task<IResult> Register(RegisterUserRequest registration, HttpContext context);

    public Task<IResult> Login(LoginRequest login);

    public Task<IResult> Refresh(RefreshRequest refreshRequest);

    public Task<IResult> ConfirmEmail(string userId, string code, string? changedEmail);

    public Task<IResult> ResendConfirmationEmail(ResendConfirmationEmailRequest resendRequest, HttpContext context);

    public Task<IResult> ForgotPassword(ForgotPasswordRequest resetRequest);

    public Task<IResult> ResetPassword(ResetPasswordRequest resetRequest);

    public Task<IResult> Manage2Fa(TwoFactorRequest tfaRequest, ClaimsPrincipal claimsPrincipal);

    public Task<IResult> Get2FaInfo(string email);

    public Task<IResult> GetInfo(ClaimsPrincipal claimsPrincipal);

    public Task<IResult> PostInfo(InfoRequest infoRequest, HttpContext context);

    public Task<IResult> SigninVk(OauthCodeRequest oauthCodeRequest);
}
