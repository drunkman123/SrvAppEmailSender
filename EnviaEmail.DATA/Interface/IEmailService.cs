using EnviaEmail.DATA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnviaEmail.DATA.Interface
{
    public interface IEmailService
    {
        bool SendEmail(EnvioEmailModel model);
        bool SaveAttachment(SaveAttachmentModel model);
        void DeleteAttachment(string attachmentPath);
        string GenerateAndSendRandomToken(EmailTokenModel email);
    }
}
