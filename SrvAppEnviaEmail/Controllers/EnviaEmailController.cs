using EnviaEmail.DATA.Model;
using EnviaEmail.DATA.Repositories;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult SendEmail([FromBody] EnvioEmailModel model)
        {
            _EnviaEmailRepo.SendEmail(model);
            return Ok(new { enviado = true, de = model.From, para = model.To, assunto = model.Subject});
        }        
        
        [HttpPost]
        public IActionResult SendEmailWithAttachment([FromBody] EnvioEmailAttachmentModel model)
        {
            _EnviaEmailRepo.SendEmail(model);
            return Ok("Email Enviado");
        }

    }
}
