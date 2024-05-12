using Microsoft.AspNetCore.Identity;

namespace Users.Services.EmailSender;

public interface IYandexEmailSender : IEmailSender<AppUser>
{
    public Task SendEmailMessage(string email, string subject, string message);
}
