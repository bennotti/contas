using Contas.Core.Services.Interfaces;
using Contas.Core.ViewModels;
using Contas.Core.ViewModels.Commons;
using Contas.Core.ViewModels.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Contas.Core.Services.Commands
{
    public class EmailServices : IEmailServices
    {
        private readonly IOptions<ServiceSettings> _appSettings;
        private readonly IOptions<EmailConfigurations> _emailSettings;

        public EmailServices(IOptions<ServiceSettings> appSettings, IOptions<EmailConfigurations> emailSettings)
        {
            _appSettings = appSettings;
            _emailSettings = emailSettings;
        }

        public async Task<SendEmailResult> EnviarEmail(string email, string assunto, string mensagem)
        {
            await EnviarEmailSmtp(email, assunto, mensagem);

            return new SendEmailResult();
        }
        async Task EnviarEmailSmtp(string para, string assunto, string mensagem)
        {
            try
            {
                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.Value.UsernameEmail, _emailSettings.Value.FromEmail)
                };

                mail.To.Add(new MailAddress(para));

                mail.Subject = assunto;
                mail.Body = mensagem;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                using (SmtpClient smtp = new SmtpClient(_emailSettings.Value.PrimaryDomain, _emailSettings.Value.PrimaryPort))
                {
                    smtp.Credentials = new NetworkCredential(_emailSettings.Value.UsernameEmail, _emailSettings.Value.UsernamePassword);
                    smtp.EnableSsl = Convert.ToBoolean(_emailSettings.Value.EnableSsl);
                    await smtp.SendMailAsync(mail);
                }
            }
            catch //(Exception ex)
            {
                //throw ex;
            }
        }
    }
}
