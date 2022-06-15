using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Rx.Domain.DTOs.Email;
using Rx.Domain.Interfaces.Email;
using Rx.Domain.Settings;

namespace Rx.Infrastructure.Shared;

public class EmailService:IEmailService
{
    private MailSettings _mailSettings;

    public EmailService(IOptions<MailSettings> mailSettings)
    {
        _mailSettings = mailSettings.Value;
    }
    public async Task SendAsync(EmailRequest request)
    {
        try
        {
            // create message
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.EmailFrom);
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.EmailFrom));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = request.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

        
            
        }
        catch (System.Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}