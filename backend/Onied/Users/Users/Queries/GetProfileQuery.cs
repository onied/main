using System.Security.Claims;
using MediatR;

namespace Users.Queries;

public record GetProfileQuery(ClaimsPrincipal User) : IRequest<IResult>;
