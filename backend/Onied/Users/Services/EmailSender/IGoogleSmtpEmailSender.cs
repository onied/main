using Microsoft.AspNetCore.Identity;
using Users.Data.Entities;

namespace Users.Services.EmailSender;

public interface IGoogleSmtpEmailSender : IEmailSender<AppUser>
{
    public Task SendEmailMessage(string email, string subject, string message);
}
