using Rx.Domain.DTOs.Email;

namespace Rx.Domain.Interfaces.Email;

public interface IEmailService
{
    Task SendAsync(EmailRequest request);
}