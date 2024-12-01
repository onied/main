using MediatR;
using Users.Dtos.Users.Request;

namespace Users.Commands;

public record RegisterCommand(RegisterUserRequest RegisterUserRequest, HttpContext HttpContext) : IRequest<IResult>;
