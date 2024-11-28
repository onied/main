using MediatR;
using Microsoft.AspNetCore.Identity.Data;

namespace Users.Commands;

public record ResetPasswordCommand(ResetPasswordRequest ResetPasswordRequest) : IRequest<IResult>;
