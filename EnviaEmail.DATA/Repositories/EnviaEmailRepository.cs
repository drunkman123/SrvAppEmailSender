using EnviaEmail.DATA.Interface;
using EnviaEmail.DATA.Model;
using EnviaEmail.DATA.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EnviaEmail.DATA.Repositories
{
    public class EnviaEmailRepository
    {
        private IHostEnvironment _hostEnvironment;
        readonly EmailService _mailService;

        //public EnviaEmailRepository(IConfiguration Configuration, IHostEnvironment hostingEnvironment, IHttpClientFactory httpClientFactory)
        public EnviaEmailRepository(IConfiguration Configuration, IHostEnvironment hostingEnvironment)
        {
            //_httpClientFactory = httpClientFactory;
            _hostEnvironment = hostingEnvironment;
            _mailService = new EmailService(Configuration);
            //_scDBPad = ConfigurationBinder.GetValue<string>(Configuration, "ConnPadrao");
            //_dbFuncEfe = ConfigurationBinder.GetValue<string>(Configuration, "FuncEfe");
        }

        public bool SendEmail(EnvioEmailModel model)
        {
            _mailService.SendEmail(model.From,model.To,model.Subject,model.Body);
            return true;
        }        
        
        public bool SendEmail(EnvioEmailAttachmentModel model)
        {
            _mailService.SendEmail(model.From, model.To, model.Subject, model.Body, model.Attachment);
            return true;
        }
        public string GerarTokenConfirmacao() 
        {
            //FALTA PROGRAMAR
            //AQUI CRIARÁ UM TOKEN RANDOMICO E DEPOIS SERÁ INSERIDO NO BANCO
            return "token";
        }
    }

}
