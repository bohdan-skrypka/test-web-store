using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WebUI.Models
{
    public class MessageServices
    {
        public async static Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var _email = "skripkabodya97@gmail.com";
                var _epass = ConfigurationManager.AppSettings["EmailPassword"];
                var _dispName = "Bodya";
                MailMessage myMesssage = new MailMessage();
                myMesssage.To.Add(email);
                myMesssage.From = new MailAddress(_email, _dispName);
                myMesssage.Subject = subject;
                myMesssage.Body = message;
                myMesssage.IsBodyHtml = true;


                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.EnableSsl = true;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(_email, _epass);
                    smtp.SendCompleted += (s, e) => { smtp.Dispose(); };
                    await smtp.SendMailAsync(myMesssage);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}