using System.Web;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Users.Services.EmailSender;

public class LoggingEmailSender(ILogger<LoggingEmailSender> logger, IConfiguration configuration)
    : IEmailSender<AppUser>
{
    public Task SendConfirmationLinkAsync(AppUser user, string email, string confirmationLink)
    {
        confirmationLink = HttpUtility.HtmlDecode(confirmationLink);
        var url = new UriBuilder(configuration.GetConnectionString("FrontendConfirmEmailUrl")!);
        url.Query = new Uri(confirmationLink).Query;
        confirmationLink = url.ToString();
        logger.LogInformation("Confirmation link for {FirstName} {LastName} sent to {email}: {confirmationLink}",
            user.FirstName, user.LastName, email, confirmationLink);
        return Task.CompletedTask;
    }

    public Task SendPasswordResetLinkAsync(AppUser user, string email, string resetLink)
    {
        resetLink = HttpUtility.HtmlDecode(resetLink);
        logger.LogInformation("Password reset link for {FirstName} {LastName} sent to {email}: {resetLink}",
            user.FirstName, user.LastName, email, resetLink);
        return Task.CompletedTask;
    }

    public Task SendPasswordResetCodeAsync(AppUser user, string email, string resetCode)
    {
        resetCode = HttpUtility.HtmlDecode(resetCode);
        var frontUrl = new UriBuilder(configuration.GetConnectionString("FrontendPasswordResetUrl")!)
        {
            Query = new QueryBuilder
            {
                { "email", email },
                { "code", resetCode }
            }.ToString()
        };
        logger.LogInformation("Password reset code link for {FirstName} {LastName} sent to {email}: {frontUrl}",
            user.FirstName, user.LastName, email, frontUrl.ToString());
        return Task.CompletedTask;
    }
}
