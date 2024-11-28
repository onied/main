using System.Security.Claims;
using MediatR;

namespace Users.Queries;

public record GetManageInfoQuery(ClaimsPrincipal User) : IRequest<IResult>;
