using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RPA_benchmarking.Class
{
    public class Email
    {
        public Email(string provedor, string username, string password)
        {
            Provedor = provedor ?? throw new ArgumentNullException(nameof(provedor));
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Password = password ?? throw new ArgumentNullException(nameof(password));
        }

        public string Provedor { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public void SendEmail(List<string> emailsTo, string subject, string body)
        {
            var menssage = PrepareteMenssage(emailsTo, subject, body);
            SendEmailBySmtp(menssage);
        }

        private MailMessage PrepareteMenssage(List<string> emailsTo, string subject, string body)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress(Username);

            foreach (var email in emailsTo)
            {
                if (ValidateEmail(email))
                {
                    mail.To.Add(email);
                }
            }

            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            return mail;

        }

        private bool ValidateEmail(string email)
        {
            Regex expretion = new Regex(@"\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}");

            if (expretion.IsMatch(email))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SendEmailBySmtp(MailMessage menssage)
        {
            SmtpClient smtp = new SmtpClient(Provedor, 587);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(Username, Password);
            smtp.Send(menssage);
            smtp.Dispose();
        }


    }
}
