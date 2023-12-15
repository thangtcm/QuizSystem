using EduQuiz_5P.Models;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EduQuiz_5P.Services
{
    public class SendMailService : IEmailSender
    {
        private readonly MailSettings mailSettings;

        private readonly ILogger<SendMailService> logger;

        public SendMailService(IOptions<MailSettings> _mailSettings, ILogger<SendMailService> _logger)
        {
            mailSettings = _mailSettings.Value;
            logger = _logger;
            logger.LogInformation("Create SendMailService");
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var builder = new BodyBuilder
            {
                HtmlBody = htmlMessage,
            };
            var message = new MimeMessage
            {
                Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail),
                Subject = subject,
                Body = builder.ToMessageBody()
            };
            message.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));
            message.To.Add(MailboxAddress.Parse(email));
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            Console.WriteLine(mailSettings.Mail + "\t" + mailSettings.Password + "\n\n\n");
            try
            {
                smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
                await smtp.SendAsync(message);
            }
            catch (Exception ex)
            {
                Directory.CreateDirectory("mailssave");
                var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                await message.WriteToAsync(emailsavefile);

                logger.LogInformation("Lỗi gửi mail, lưu tại - " + emailsavefile);
                logger.LogError(ex.Message);
            }

            smtp.Disconnect(true);

            logger.LogInformation("send mail to: " + email);

        }
    }
}
