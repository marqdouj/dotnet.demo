using Marqdouj.DotNet.General.Extensions;
using MimeKit;
using System.Text;

namespace AspireDemo.ApiService.Services
{
    internal static class EmailServiceExtensions
    {
        public static void ConfigureEmailService(this WebApplicationBuilder builder)
        {
            builder.AddMailKitService();

            //Example on how to configure different email configurations

            //if (builder.Environment.IsDevelopment())
            //{
            //    var useAzureEmailInDev = false; // change to true to test azure email in IDE

            //    if (useAzureEmailInDev)
            //    {
            //        builder.AddAzureMailService();
            //    }
            //    else
            //    {
            //        builder.AddMailKitService();
            //    }
            //}
            //else
            //{
            //    builder.AddAzureMailService();
            //}
        }

        public static MimeMessage BuildMimeMessage(this IEmailService service, string recipient, string subject, string body, bool bodyIsHtml)
        {
            var bodyType = bodyIsHtml ? "html" : "plain";

            var message = new MimeMessage
            {
                Sender = service.Settings.Sender
            };
            message.To.Add(new MailboxAddress("", recipient));
            message.Subject = subject;
            message.Body = new TextPart(bodyType) { Text = body };

            return message;
        }

        public static MimeMessage BuildNewsletterMessage(this IEmailService service, string recipient, bool subscribe)
        {
            var subject = subscribe ? "Welcome to our newsletter!" : "You are unsubscribed from our newsletter!";
            var body = subscribe ? "Thank you for subscribing to our newsletter!" : "Sorry to see you go. We hope you will come back soon!";

            var message = new MimeMessage
            {
                Sender = service.Settings.Sender
            };
            message.To.Add(new MailboxAddress("", recipient));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            return message;
        }

        public static MimeMessage BuildErrorMessage(this EmailServiceSettings settings, Exception exception)
        {
            var message = new MimeMessage
            {
                Sender = settings.Sender
            };
            message.To.AddRange(settings.ErrorRecipients);
            message.Subject = $"DEMO Exception - {exception.Source}";

            var body = new StringBuilder();
            body.AppendLine("The following exception has occurred:");
            body.AppendLine();
            body.AppendLine($"{exception.ToMessage()}");
            message.Body = new TextPart("plain") { Text = body.ToString() };

            return message;
        }

        public static WebApplicationBuilder AddMailKitService(this WebApplicationBuilder builder) 
        {
            var settings = builder.BuildSettings("MailKitEmail");

            builder.AddMailKitClient("maildev");
            builder.Services.AddSingleton(settings);
            builder.Services.AddScoped<IEmailService, MailKitService>();
            return builder;
        }

        private static EmailServiceSettings BuildSettings(this WebApplicationBuilder builder, string section)
        {
            var config = new EmailServiceSettingsConfig();
            var jsonSettings = builder.Configuration.GetRequiredSection(section);
            jsonSettings.Bind(config);

            var settings = new EmailServiceSettings(
                new MailboxAddress("DEMO", config.Sender),
                [.. config.ErrorRecipients!.Select(e => new MailboxAddress("", e))],
                config.ConnectionString);

            return settings;
        }
    }
}
