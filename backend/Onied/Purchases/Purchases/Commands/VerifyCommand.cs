using MediatR;
using Purchases.Dtos.Requests;

namespace Purchases.Commands;

public record VerifyCommand(VerifyTokenRequestDto dto) : IRequest<IResult>;
