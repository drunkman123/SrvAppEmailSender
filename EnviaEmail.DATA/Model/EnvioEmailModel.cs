using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnviaEmail.DATA.Model
{
    public class EnvioEmailModel
    {
        public string From { get; set; }
        public List<string> To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
    public class EnvioEmailAttachmentModel : EnvioEmailModel
    {
        public string Attachment { get; set; }
    }
}
