using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WM_Attendance_System.Settings;
using WM_Attendance_System.Models;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace WM_Attendance_System.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            MailAddress from = new MailAddress(_mailSettings.Mail, _mailSettings.DisplayName, Encoding.UTF8);
            MailAddress to = new MailAddress(mailRequest.ToEmail);
            MailMessage email = new MailMessage(from, to);
            email.Subject = mailRequest.Subject;
            email.Body = mailRequest.Body;
            email.BodyEncoding = Encoding.UTF8;
            email.IsBodyHtml = true;
            if (mailRequest.MailAttachment is not null)
            {
                email.Attachments.Add(mailRequest.MailAttachment);
            }
            SmtpClient client = new SmtpClient(_mailSettings.Host, _mailSettings.Port);
            NetworkCredential networkCredential = new NetworkCredential(_mailSettings.Mail, _mailSettings.Password);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = networkCredential;
            try
            {
                await client.SendMailAsync(email);
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
            finally
            {
                client.Dispose();
            }
        }
    }
}
