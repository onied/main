using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Users.Dtos.Users.Request;
using Users.Dtos.VkOauth.Request;
using Users.Services.UsersService;

namespace Users.Controllers;

[ApiController]
[Route("/api/v1/[action]")]
public class UsersController(IUsersService usersService) : ControllerBase
{
    [HttpPost]
    public Task<IResult> Register([FromBody] RegisterUserRequest registration)
    {
        return usersService.Register(registration, HttpContext);
    }

    [HttpPost]
    public Task<IResult> Login([FromBody] LoginRequest login)
    {
        return usersService.Login(login);
    }

    [HttpPost]
    public Task<IResult> Refresh([FromBody] RefreshRequest refreshRequest)
    {
        return usersService.Refresh(refreshRequest);
    }

    [HttpGet]
    public Task<IResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string code,
        [FromQuery] string? changedEmail)
    {
        return usersService.ConfirmEmail(userId, code, changedEmail);
    }

    [HttpPost]
    public Task<IResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailRequest resendRequest)
    {
        return usersService.ResendConfirmationEmail(resendRequest, HttpContext);
    }

    [HttpPost]
    public Task<IResult> ForgotPassword([FromBody] ForgotPasswordRequest resetRequest)
    {
        return usersService.ForgotPassword(resetRequest);
    }

    [HttpPost]
    public Task<IResult> ResetPassword([FromBody] ResetPasswordRequest resetRequest)
    {
        return usersService.ResetPassword(resetRequest);
    }

    [HttpPost]
    [ActionName("manage/2fa")]
    public Task<IResult> Manage2Fa([FromBody] TwoFactorRequest tfaRequest)
    {
        return usersService.Manage2Fa(tfaRequest, User);
    }

    [HttpGet]
    [ActionName("manage/info")]
    public Task<IResult> GetInfo()
    {
        return usersService.GetInfo(User);
    }

    [HttpPost]
    [ActionName("manage/info")]
    public Task<IResult> PostInfo([FromBody] InfoRequest infoRequest)
    {
        return usersService.PostInfo(infoRequest, HttpContext);
    }

    [HttpPost]
    public Task<IResult> SigninVk([FromBody] OauthCodeRequest oauthCodeRequest)
    {
        return usersService.SigninVk(oauthCodeRequest);
    }
}
