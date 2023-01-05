using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnviaEmail.DATA.Model
{
    public class EmailTokenModel
    {
        public string email { get; set; }

    }

    public class GenerateTokenModel : EmailTokenModel
    {
        public string? token { get; set; }
        public DateTime? data_insert { get; set; }
    }
}
