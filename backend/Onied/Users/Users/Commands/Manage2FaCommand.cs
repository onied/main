using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;

namespace Users.Commands;

public record Manage2FaCommand(TwoFactorRequest TwoFactorRequest, ClaimsPrincipal User) : IRequest<IResult>;
