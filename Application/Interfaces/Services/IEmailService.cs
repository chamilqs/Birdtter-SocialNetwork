using SocialNetwork.Core.Application.DTOs.Email;
using SocialNetwork.Core.Domain.Settings;

namespace SocialNetwork.Core.Application.Interfaces.Services
{
    public interface IEmailService
    {
        public MailSettings MailSettings { get; }
        Task SendAsync(EmailRequest request);
    }
}
