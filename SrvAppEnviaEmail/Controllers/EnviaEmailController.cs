using EnviaEmail.DATA.Model;
using EnviaEmail.DATA.Repositories;
using EnviaEmail.DATA.Service;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace SrvAppEnviaEmail.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EnviaEmailController : Controller
    {
        private IHostEnvironment _hostingEnvironment;
        //private readonly EnviaEmailRepository _EnviaEmailRepo;
        private readonly EmailService _EnviaEmailSvc;

        //public EnviaEmailController(IHostEnvironment hostingEnvironment, IConfiguration Configuration, IHttpClientFactory httpClientFactory)
        public EnviaEmailController(IHostEnvironment hostingEnvironment, IConfiguration Configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            //_EnviaEmailRepo = new EnviaEmailRepository(Configuration, hostingEnvironment, httpClientFactory);
            //_EnviaEmailRepo = new EnviaEmailRepository(Configuration, hostingEnvironment);
            _EnviaEmailSvc = new EmailService(Configuration, hostingEnvironment);

        }

        [HttpPost]
        [Produces("application/json")]

        public ActionResult<bool> SendEmail([FromBody] EnvioEmailModel model)
        {
            bool result = true;
            try
            {
                result = _EnviaEmailSvc.SendEmail(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
            return Ok(new { enviado = result, para = model.to, assunto = model.subject, anexo = model.attachmentName != null && model.attachmentName.Count() > 0 ? "Sim" : "Não" });
        }

        [HttpPost]
        public ActionResult<string> SaveAttachment([FromBody] SaveAttachmentModel model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    if (((model.base64.Length * 3 / 4)) > 2600000)
                    {
                        return "Tamanho do arquivo acima do permitido (2,5mb).";
                    }
                    if (_EnviaEmailSvc.SaveAttachment(model))
                    {
                        return Ok(new { upload = true });
                    };
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
            return Ok(new { upload = false });

        }

        [HttpPost]
        public ActionResult<string> GenerateRandomToken([FromBody] EmailTokenModel model)
        {
            string token = string.Empty;
            try
            {
                token = _EnviaEmailSvc.GenerateAndSendRandomToken(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
            return Ok(new { success = true, token = token });
        }

    }
}
