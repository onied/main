using MediatR;

namespace Users.Commands;

public record ConfirmEmailCommand(string UserId, string Code, string? ChangedEmail) : IRequest<IResult>;
