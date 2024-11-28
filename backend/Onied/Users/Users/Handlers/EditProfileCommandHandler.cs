using MediatR;
using Users.Commands;
using Users.Services.ProfileService;

namespace Users.Handlers;

public class EditProfileCommandHandler(IProfileService profileService) : IRequestHandler<EditProfileCommand, IResult>
{
    public async Task<IResult> Handle(EditProfileCommand request, CancellationToken cancellationToken)
    {
        return await profileService.EditProfile(request.ProfileChangedRequest, request.User);
    }
}
