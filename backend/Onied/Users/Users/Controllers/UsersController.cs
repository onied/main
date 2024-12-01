using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Users.Commands;
using Users.Dtos.Users.Request;
using Users.Dtos.VkOauth.Request;
using Users.Queries;

namespace Users.Controllers;

[ApiController]
[Route("/api/v1/[action]")]
public class UsersController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IResult> Register([FromBody] RegisterUserRequest registration)
    {
        return await sender.Send(new RegisterCommand(registration, HttpContext));
    }

    [HttpPost]
    public async Task<IResult> Login([FromBody] LoginRequest login)
    {
        return await sender.Send(new LoginCommand(login));
    }

    [HttpPost]
    public async Task<IResult> Refresh([FromBody] RefreshRequest refreshRequest)
    {
        return await sender.Send(new TokenRefreshCommand(refreshRequest));
    }

    [HttpGet]
    public async Task<IResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string code,
        [FromQuery] string? changedEmail)
    {
        return await sender.Send(new ConfirmEmailCommand(userId, code, changedEmail));
    }

    [HttpPost]
    public async Task<IResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailRequest resendRequest)
    {
        return await sender.Send(new ResendConfirmationEmailCommand(resendRequest, HttpContext));
    }

    [HttpPost]
    public async Task<IResult> ForgotPassword([FromBody] ForgotPasswordRequest resetRequest)
    {
        return await sender.Send(new ForgotPasswordCommand(resetRequest));
    }

    [HttpPost]
    public async Task<IResult> ResetPassword([FromBody] ResetPasswordRequest resetRequest)
    {
        return await sender.Send(new ResetPasswordCommand(resetRequest));
    }

    [HttpPost]
    [ActionName("manage/2fa")]
    public async Task<IResult> Manage2Fa([FromBody] TwoFactorRequest tfaRequest)
    {
        return await sender.Send(new Manage2FaCommand(tfaRequest, User));
    }

    [HttpGet]
    [ActionName("manage/info")]
    public async Task<IResult> GetInfo()
    {
        return await sender.Send(new GetManageInfoQuery(User));
    }

    [HttpPost]
    [ActionName("manage/info")]
    public async Task<IResult> PostInfo([FromBody] InfoRequest infoRequest)
    {
        return await sender.Send(new ManageInfoCommand(infoRequest, HttpContext));
    }

    [HttpPost]
    public async Task<IResult> SigninVk([FromBody] OauthCodeRequest oauthCodeRequest)
    {
        return await sender.Send(new SignInVkCommand(oauthCodeRequest));
    }
}
