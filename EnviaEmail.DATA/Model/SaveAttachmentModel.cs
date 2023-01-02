using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EnviaEmail.DATA.Model
{
    public class SaveAttachmentModel
    {
        //private string? _cpf;

        [RegularExpression(@"^[a-zA-Z0-9\+/]*={0,3}$",
        ErrorMessage = "Base64 inválido.")]
        public string base64 { get; set; }

        [RegularExpression(@"^(?!.*\.exe$)[\w\-. ]+\.[\w]+$",
         ErrorMessage = "Caracteres inválidos, extensão (.exe) proibida ou falta de extensão no nome do arquivo.")]
        public string fileName { get; set; }

        //[StringLength(11,
        // ErrorMessage = "CPF inválido.")]
        //public string cpf
        //{
        //    get => _cpf;
        //    set
        //    {
        //        _cpf = value.Replace(".","").Replace("-","").Trim();
        //    }
        //}
        //}
    }
}

