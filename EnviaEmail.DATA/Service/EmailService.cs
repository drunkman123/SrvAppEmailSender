using EnviaEmail.DATA.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EnviaEmail.DATA.Service
{
    public class EmailService : IEmailService
    {
        private readonly string _smtp = "";
        private readonly string _user = "";
        private readonly string _pass = "";

        public EmailService(IConfiguration Configuration)
        {
            _smtp = Configuration.GetValue<string>("smtp");
            _user = Configuration.GetValue<string>("user");
            _pass = Configuration.GetValue<string>("pass");
        }
        public bool SendEmail(string from, List<string> to, string subject, string body)
        {
            bool res = false;
            // Instancia da Classe de Mensagem
            MailMessage _mailMessage = new MailMessage();

            // Remetente
            _mailMessage.From = new MailAddress(from);

            // Destinatarios múltiplos ocultos
            foreach (string em in to)
            {
                _mailMessage.Bcc.Add(new MailAddress(em));
                //_mailMessage.To.Add(em); //caso n queira ocultar
            }
            // Assunto
            _mailMessage.Subject = subject;

            // A mensagem é do tipo HTML ou Texto Puro?
            _mailMessage.IsBodyHtml = true;

            // Corpo da Mensagem
            _mailMessage.Body = body;          

            //codificação do assunto do email para que os caracteres acentuados serem reconhecidos.
            _mailMessage.SubjectEncoding = Encoding.GetEncoding("ISO-8859-1");

            //codificação do corpo do emailpara que os caracteres acentuados serem reconhecidos.
            _mailMessage.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");

            // Estancia a Classe de Envio
            SmtpClient _smtpClient = new SmtpClient(_smtp);

            // Credencial para envio por SMTP Seguro (Quando o servidor exige autenticação)
            _smtpClient.Credentials = new NetworkCredential(_user, _pass);

            // Envia a mensagem
            try
            {
                _smtpClient.Send(_mailMessage);
                res = true;
            }
            catch (Exception ex)
            {
                res = false;
                throw;
            }
            finally
            {
                _smtpClient.Dispose();
                _mailMessage.Dispose();
            }

            return res;
        }        
        public bool SendEmail(string from, List<string> to, string subject, string body, string attachment)
        {
            throw new NotImplementedException();
        }
    }
}
