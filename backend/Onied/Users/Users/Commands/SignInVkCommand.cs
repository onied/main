using MediatR;
using Users.Dtos.VkOauth.Request;

namespace Users.Commands;

public record SignInVkCommand(OauthCodeRequest OauthCodeRequest) : IRequest<IResult>;
