using EnviaEmail.DATA.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
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
        private readonly IHostEnvironment _hostingEnvironment;
        private readonly string _smtp = "";
        private readonly string _user = "";
        private readonly string _pass = "";

        public EmailService(IConfiguration Configuration, IHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _smtp = Configuration.GetValue<string>("smtp");
            _user = Configuration.GetValue<string>("user");
            _pass = Configuration.GetValue<string>("pass");
        }

        public bool SendEmail(string from, List<string> to, string subject, string body, string[]? attachment)
        {
            bool res = false;
            #region 
            // Instancia da Classe de Mensagem
            MailMessage _mailMessage = new MailMessage();

            // Remetente
            _mailMessage.From = new MailAddress(from);

            // Destinatario
            foreach (string em in to)
            {
                _mailMessage.Bcc.Add(new MailAddress(em));
                //_mailMessage.To.Add(em);
            }
            // Assunto
            _mailMessage.Subject = subject;

            // A mensagem é do tipo HTML ou Texto Puro?
            _mailMessage.IsBodyHtml = true;

            // Corpo da Mensagem
            _mailMessage.Body = body;


            ////Caminho do arquivo a ser enviado como anexo
            if(attachment != null && attachment.Count() > 0)
            {
                foreach (string em in attachment)
                {
                    string arquivo = _hostingEnvironment.ContentRootPath + @"\tmp\" + em;

                    //// Cria o anexo para o e-mail
                    bool ExisteArq = File.Exists(arquivo);
                    if (ExisteArq)
                    {
                        Attachment Anexo = new Attachment(arquivo, System.Net.Mime.MediaTypeNames.Application.Octet);
                        // Anexa o arquivo a mensagemn
                        _mailMessage.Attachments.Add(Anexo);
                    }
                }
                
            }
            

            //codificação do assunto do email para que os caracteres acentuados serem reconhecidos.
            _mailMessage.SubjectEncoding = Encoding.GetEncoding("ISO-8859-1");

            //codificação do corpo do emailpara que os caracteres acentuados serem reconhecidos.
            _mailMessage.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");

            // Instancia a Classe de Envio
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
                foreach (string em in attachment)
                {
                    string arquivo = _hostingEnvironment.ContentRootPath + @"\tmp\" + em;
                    DeleteAttachment(arquivo);
                }
            }
            #endregion

            return res;
            //Attachment attachment = new Attachment("path/to/attachment.pdf");
            //message.Attachments.Add(attachment);
        }
        public bool SaveAttachment(string base64, string fileName)
        {
            try
            {
                // Decode the base64 string and get the binary data
                byte[] data = Convert.FromBase64String(base64);

                // Create a new file and write the binary data to it
                using (FileStream fileStream = File.Create(Path.Combine(_hostingEnvironment.ContentRootPath + "\\tmp\\" + fileName)))
                {
                    fileStream.Write(data, 0, data.Length);
                    fileStream.Flush();
                }
            }
            catch(Exception ex)
            {
                throw;
            }

           
            return true;
        }        
        
        public void DeleteAttachment(string attachmentPath)
        {
            File.Delete(attachmentPath);
        }
    }
}
