using MediatR;

namespace Support.Queries;

public record GetProfileQuery(Guid? UserId) : IRequest<IResult>;
