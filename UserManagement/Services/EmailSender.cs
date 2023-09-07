using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Net;
using System.Net.Mail;

namespace UserManagement.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var FromMail = new MailAddress("mohamedbazedy3010331@outlook.sa");
            var FromPassword = "Bazedy1$";
            var Message = new MailMessage();
            Message.From = FromMail;
            Message.Subject = subject;
            Message.To.Add(email);
            Message.Body = $"<html><body>{ htmlMessage}</html></body>";
            Message.IsBodyHtml = true;
            var smtp = new SmtpClient("smtp-mail.outlook.com", 587)
            {
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(FromMail.Address, FromPassword),
                EnableSsl = true,


            };
            await smtp.SendMailAsync(Message);
        }
        
    }
}
