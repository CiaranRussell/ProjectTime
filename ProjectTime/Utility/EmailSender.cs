using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;


namespace ProjectTime.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                // Get from & to email addresses and email body from IEmailSender 
                var emailToSend = new MimeMessage();
                emailToSend.From.Add(MailboxAddress.Parse("donotreply.projecttime@gmail.com"));
                emailToSend.To.Add(MailboxAddress.Parse(email));
                emailToSend.Subject = subject;
                emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };

                // Connect through smpt mailkit and send mail 
                using (var emailClient = new SmtpClient())
                {
                    emailClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    emailClient.Authenticate("donotreply.projecttime@gmail.com", "egzbhuugleukwbld");
                    emailClient.Send(emailToSend);
                    emailClient.Disconnect(true);
                }
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }
    }
}
