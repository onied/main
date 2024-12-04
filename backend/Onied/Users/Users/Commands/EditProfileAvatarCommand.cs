using System.Security.Claims;
using MediatR;
using Users.Dtos.Profile.Request;

namespace Users.Commands;

public record EditProfileAvatarCommand(AvatarChangedRequest AvatarChangedRequest, ClaimsPrincipal User)
    : IRequest<IResult>;
