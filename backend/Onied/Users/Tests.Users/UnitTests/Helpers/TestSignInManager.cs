using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Users.Data.Entities;

namespace Tests.Users.UnitTests.Helpers;

public class TestSignInManager : SignInManager<AppUser>
{
    public TestSignInManager()
        : base(new TestUserManager(),
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<AppUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<AppUser>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object,
            new Mock<IUserConfirmation<AppUser>>().Object)
    {
    }
}
