using MediatR;
using Microsoft.AspNetCore.Identity.Data;

namespace Users.Commands;

public record TokenRefreshCommand(RefreshRequest RefreshRequest) : IRequest<IResult>;
