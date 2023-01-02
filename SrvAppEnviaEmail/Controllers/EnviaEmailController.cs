using EnviaEmail.DATA.Model;
using EnviaEmail.DATA.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace SrvAppEnviaEmail.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EnviaEmailController : Controller
    {
        private IHostEnvironment _hostingEnvironment;
        private readonly EnviaEmailRepository _EnviaEmailRepo;

        //public EnviaEmailController(IHostEnvironment hostingEnvironment, IConfiguration Configuration, IHttpClientFactory httpClientFactory)
        public EnviaEmailController(IHostEnvironment hostingEnvironment, IConfiguration Configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            //_EnviaEmailRepo = new EnviaEmailRepository(Configuration, hostingEnvironment, httpClientFactory);
            _EnviaEmailRepo = new EnviaEmailRepository(Configuration, hostingEnvironment);

        }

        [HttpPost]
        [Produces("application/json")]

        public ActionResult<bool> SendEmail([FromBody] EnvioEmailModel model)
        {

            _EnviaEmailRepo.SendEmail(model);
            return Ok(new { enviado = true, para = model.To, assunto = model.Subject, anexo = model.attachmentName.Count() > 0 ? "Sim" : "Não" });
        }

        [HttpPost]
        public ActionResult<string> SaveAttachment([FromBody] SaveAttachmentModel model)
        {

            if (ModelState.IsValid)
            {
                if (((model.base64.Length * 3 / 4)) > 2600000)
                {
                    return "Tamanho do arquivo acima do permitido (2,5mb).";
                }
                if (_EnviaEmailRepo.SaveAttachment(model))
                {
                    return Ok(new { upload = true });
                };
            }
            return Ok(new { upload = false });

        }

    }
}
