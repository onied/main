using System.Web;
using Microsoft.AspNetCore.Identity;

namespace Users.Services.EmailSender;

public class LoggingEmailSender(ILogger<LoggingEmailSender> logger) : IEmailSender<AppUser>
{
    public Task SendConfirmationLinkAsync(AppUser user, string email, string confirmationLink)
    {
        confirmationLink = HttpUtility.HtmlDecode(confirmationLink);
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
        logger.LogInformation("Password reset code for {FirstName} {LastName} sent to {email}: {resetCode}",
            user.FirstName, user.LastName, email, resetCode);
        return Task.CompletedTask;
    }
}
