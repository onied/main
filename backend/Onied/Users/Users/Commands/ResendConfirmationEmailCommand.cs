using MediatR;
using Microsoft.AspNetCore.Identity.Data;

namespace Users.Commands;

public record ResendConfirmationEmailCommand(
    ResendConfirmationEmailRequest ResendConfirmationEmailRequest,
    HttpContext HttpContext) : IRequest<IResult>;
