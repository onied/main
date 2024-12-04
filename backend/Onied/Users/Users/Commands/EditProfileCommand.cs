using System.Security.Claims;
using MediatR;
using Users.Dtos.Profile.Request;

namespace Users.Commands;

public record EditProfileCommand(ProfileChangedRequest ProfileChangedRequest, ClaimsPrincipal User) : IRequest<IResult>;
