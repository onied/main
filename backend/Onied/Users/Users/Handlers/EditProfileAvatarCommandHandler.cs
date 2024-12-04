using MediatR;
using Users.Commands;
using Users.Services.ProfileService;

namespace Users.Handlers;

public class EditProfileAvatarCommandHandler(IProfileService profileService)
    : IRequestHandler<EditProfileAvatarCommand, IResult>
{
    public async Task<IResult> Handle(EditProfileAvatarCommand request, CancellationToken cancellationToken)
    {
        return await profileService.Avatar(request.AvatarChangedRequest, request.User);
    }
}
