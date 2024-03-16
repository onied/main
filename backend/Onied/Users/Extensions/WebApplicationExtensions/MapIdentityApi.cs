using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Users.Dtos;

namespace Users.Extensions.WebApplicationExtensions;

public static class MapIdentityApi
{
    private static readonly EmailAddressAttribute _emailAddressAttribute = new();

    public static IEndpointRouteBuilder MapCustomIdentityApi(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var emailSender = endpoints.ServiceProvider.GetRequiredService<IEmailSender<AppUser>>();
        var linkGenerator = endpoints.ServiceProvider.GetRequiredService<LinkGenerator>();

        var finalPattern = "/confirmEmail";
        var confirmEmailEndpointName = $"{nameof(MapIdentityApi)}-{finalPattern}";

        endpoints.MapIdentityApi<AppUser>();
        endpoints.MapPost("/register", async Task<Results<Ok, ValidationProblem>>
            ([FromBody] RegisterUserDto registration, HttpContext context, [FromServices] IServiceProvider sp) =>
        {
            var userManager = sp.GetRequiredService<UserManager<AppUser>>();

            if (!userManager.SupportsUserEmail)
                throw new NotSupportedException($"{nameof(MapIdentityApi)} requires a user store with email support.");

            var userStore = sp.GetRequiredService<IUserStore<AppUser>>();
            var emailStore = (IUserEmailStore<AppUser>)userStore;
            var email = registration.Email;

            if (string.IsNullOrEmpty(email) || !_emailAddressAttribute.IsValid(email))
                return CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(email)));

            var user = new AppUser();
            await userStore.SetUserNameAsync(user, email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, email, CancellationToken.None);
            user.Gender = registration.Gender;
            user.FirstName = registration.FirstName;
            user.LastName = registration.LastName;
            var result = await userManager.CreateAsync(user, registration.Password);

            if (!result.Succeeded) return CreateValidationProblem(result);

            await SendConfirmationEmailAsync(user, userManager, context, email);
            return TypedResults.Ok();
        }).WithOrder(-1);

        async Task SendConfirmationEmailAsync(AppUser user, UserManager<AppUser> userManager, HttpContext context,
            string email, bool isChange = false)
        {
            if (confirmEmailEndpointName is null)
                throw new NotSupportedException("No email confirmation endpoint was registered!");

            var code = isChange
                ? await userManager.GenerateChangeEmailTokenAsync(user, email)
                : await userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var userId = await userManager.GetUserIdAsync(user);
            var routeValues = new RouteValueDictionary
            {
                ["userId"] = userId,
                ["code"] = code
            };

            if (isChange)
                // This is validated by the /confirmEmail endpoint on change.
                routeValues.Add("changedEmail", email);

            var confirmEmailUrl = linkGenerator.GetUriByName(context, confirmEmailEndpointName, routeValues)
                                  ?? throw new NotSupportedException(
                                      $"Could not find endpoint named '{confirmEmailEndpointName}'.");

            await emailSender.SendConfirmationLinkAsync(user, email, HtmlEncoder.Default.Encode(confirmEmailUrl));
        }

        return endpoints;
    }

    private static ValidationProblem CreateValidationProblem(IdentityResult result)
    {
        // We expect a single error code and description in the normal case.
        // This could be golfed with GroupBy and ToDictionary, but perf! :P
        Debug.Assert(!result.Succeeded);
        var errorDictionary = new Dictionary<string, string[]>(1);

        foreach (var error in result.Errors)
        {
            string[] newDescriptions;

            if (errorDictionary.TryGetValue(error.Code, out var descriptions))
            {
                newDescriptions = new string[descriptions.Length + 1];
                Array.Copy(descriptions, newDescriptions, descriptions.Length);
                newDescriptions[descriptions.Length] = error.Description;
            }
            else
            {
                newDescriptions = [error.Description];
            }

            errorDictionary[error.Code] = newDescriptions;
        }

        return TypedResults.ValidationProblem(errorDictionary);
    }
}
