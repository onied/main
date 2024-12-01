using MediatR;
using Microsoft.AspNetCore.Identity.Data;

namespace Users.Commands;

public record ForgotPasswordCommand(ForgotPasswordRequest ForgotPasswordRequest) : IRequest<IResult>;
