using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;

namespace Users.Commands;

public record ManageInfoCommand(InfoRequest InfoRequest, HttpContext HttpContext) : IRequest<IResult>;
