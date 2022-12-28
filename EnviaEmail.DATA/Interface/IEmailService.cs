using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnviaEmail.DATA.Interface
{
    public interface IEmailService
    {
        bool SendEmail(string from, List<string> to, string subject, string body);
        bool SendEmail(string from, List<string> to, string subject, string body, string attachment);
    }
}
