using MimeKit;

namespace AspireDemo.ApiService.Services
{
    internal record EmailServiceSettings(
        MailboxAddress Sender,
        List<MailboxAddress> ErrorRecipients,
        string? ConnectionString);
}
