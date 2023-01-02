using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnviaEmail.DATA.Model
{
    public class EnvioEmailModel
    {
        //private string from = "noreply@policiamilitar.sp.gov.br";
        //public string From { get { return from; } set { from = value; } }
        public string From = "xxxxxx@zxxxxx";

        [Required]
        [EnsureMaximumElements(500, ErrorMessage="Máximo de destinatários excedido, limite 500")]
        public List<string> To { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
        public string[]? attachmentName { get; set; }
    }
    public class EnsureMaximumElementsAttribute : ValidationAttribute
    {
        private readonly int _maxElements;
        public EnsureMaximumElementsAttribute(int maxElements)
        {
            _maxElements = maxElements;
        }

        public override bool IsValid(object value)
        {
            var list = value as IList;
            if (list != null)
            {
                return list.Count < _maxElements;
            }
            return false;
        }
    }
}
