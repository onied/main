using System.Web;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http.Extensions;
using MimeKit;

namespace Users.Services.EmailSender;

public class GoogleSmtpEmailSender(ILogger<GoogleSmtpEmailSender> logger, IConfiguration configuration) : IGoogleSmtpEmailSender
{
    public async Task SendConfirmationLinkAsync(AppUser user, string email, string confirmationLink)
    {
        confirmationLink = HttpUtility.HtmlDecode(confirmationLink);
        var url = new UriBuilder(configuration.GetConnectionString("FrontendConfirmEmailUrl")!);
        url.Query = new Uri(confirmationLink).Query;
        confirmationLink = url.ToString();

        var messageToSend =
            $"Для подтверждения почты перейдите по этой ссылке: <a href='{confirmationLink}'>подтвердить</a>";

        await SendEmailMessage(email, "Подтверждение почты", messageToSend);
        
        logger.LogInformation("Confirmation link for {FirstName} {LastName} sent to {email}: {confirmationLink}",
            user.FirstName, user.LastName, email, confirmationLink);
    }

    public async Task SendPasswordResetLinkAsync(AppUser user, string email, string resetLink)
    {
        resetLink = HttpUtility.HtmlDecode(resetLink);
        var messageToSend = $"Для сброса пароля перейдите по этой ссылке: <a href='{resetLink}'>сбросить</a>";
        await SendEmailMessage(email, "Сброс пароля для OniEd", messageToSend);
        
        logger.LogInformation("Password reset link for {FirstName} {LastName} sent to {email}: {resetLink}",
            user.FirstName, user.LastName, email, resetLink);
    }

    public async Task SendPasswordResetCodeAsync(AppUser user, string email, string resetCode)
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
        var messageToSend = $"Для сброса пароля перейдите по этой ссылке: <a href='{frontUrl}'>сбросить</a>";
        await SendEmailMessage(email, "Сброс пароля для OniEd", messageToSend);
        
        logger.LogInformation("Password reset code link for {FirstName} {LastName} sent to {email}: {frontUrl}",
            user.FirstName, user.LastName, email, frontUrl.ToString());
    }

    public async Task SendEmailMessage(string email, string subject, string message)
    {
        using var emailMessage = new MimeMessage();
        
        emailMessage.From.Add(new MailboxAddress(configuration["MailSettings:SenderName"],
            configuration["MailSettings:Address"]));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message
        };

        using var client = new SmtpClient();
        
        await client.ConnectAsync(configuration["MailSettings:Host"],
            int.Parse(configuration["MailSettings:Port"]!), true);
        await client.AuthenticateAsync(configuration["MailSettings:Address"],
            configuration["MailSettings:ApplicationPassword"]);
        await client.SendAsync(emailMessage);
 
        await client.DisconnectAsync(true);
    }
}
