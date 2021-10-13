using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;

namespace Riode.Application.Core.Extensions
{
    static public partial class Extension
    {
        static public bool SendEmail(this IConfiguration configuration, string to, string subject, string body, bool appendcc = false)
        {

            try
            {
                string displayName = configuration["Email:displayname"];
                string smptpserver = configuration["Email:smptpserver"];
                int smptport = Convert.ToInt32(configuration["Email:smptport"]);
                string fromMail = configuration["Email:username"];
                string password = configuration["Email:password"];
                string cc = configuration["Email:cc"];

                using (MailMessage message = new MailMessage(new MailAddress(fromMail, displayName), new MailAddress(to)))
                {
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;
                    if (!string.IsNullOrEmpty(cc) && appendcc)
                    {
                        message.CC.Add(cc);
                    }

                    SmtpClient smptpClient = new SmtpClient(smptpserver, smptport);
                    smptpClient.Credentials = new NetworkCredential(fromMail, password);
                    smptpClient.EnableSsl = true;
                    smptpClient.Send(message);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
