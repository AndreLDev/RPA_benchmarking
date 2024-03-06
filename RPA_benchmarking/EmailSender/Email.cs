using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RPA_benchmarking.EmailSender
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

        public async Task SendEmailAsync(List<string> emailsTo, string subject, string body)
        {
            var message = PrepareMessage(emailsTo, subject, body);
            await SendEmailBySmtpAsync(message);
        }

        private MailMessage PrepareMessage(List<string> emailsTo, string subject, string body)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress(Username);

            foreach (var email in emailsTo)
            {
                if (ValidateEmail(email))
                {
                    mail.To.Add(email);
                }
                else
                {
                    Console.WriteLine("Email: " + email + " Não é valido");
                }
            }

            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            return mail;
        }

        private bool ValidateEmail(string email)
        {
            Regex expression = new Regex(@"\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}");
            return expression.IsMatch(email);
        }

        private async Task SendEmailBySmtpAsync(MailMessage message)
        {
            using (SmtpClient smtp = new SmtpClient(Provedor, 587))
            {
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(Username, Password);
                await smtp.SendMailAsync(message);
            }
        }
    }
}
