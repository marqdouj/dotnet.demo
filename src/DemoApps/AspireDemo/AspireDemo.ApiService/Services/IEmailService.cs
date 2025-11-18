using MimeKit;

namespace AspireDemo.ApiService.Services
{
    internal interface IEmailService
    {
        EmailServiceSettings Settings { get; }
        Task SendEmail(MimeMessage message);
        Task SendEmail(string recipient, string subject, string body, bool bodyIsHtml);
        Task SendErrorEmail(Exception exception);
    }
}
