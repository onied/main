using MediatR;
using Microsoft.AspNetCore.Identity.Data;

namespace Users.Commands;

public record LoginCommand(LoginRequest LoginRequest) : IRequest<IResult>;
