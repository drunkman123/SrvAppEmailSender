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
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EnviaEmail.DATA.Repositories
{
    public class EnviaEmailRepository
    {
        private IHostEnvironment _hostEnvironment;
        readonly EmailService _mailService;
        //private readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy = Policy<HttpResponseMessage>.Handle<HttpRequestException>()
        //    .OrResult(x => x.StatusCode >= System.Net.HttpStatusCode.InternalServerError || x.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
        //    .RetryAsync(5);

        //public EnviaEmailRepository(IConfiguration Configuration, IHostEnvironment hostingEnvironment, IHttpClientFactory httpClientFactory)
        public EnviaEmailRepository(IConfiguration Configuration, IHostEnvironment hostingEnvironment)
        {
            //_httpClientFactory = httpClientFactory;
            _hostEnvironment = hostingEnvironment;
            _mailService = new EmailService(Configuration, hostingEnvironment);
            //_scDBPad = ConfigurationBinder.GetValue<string>(Configuration, "ConnPadrao");
            //_dbFuncEfe = ConfigurationBinder.GetValue<string>(Configuration, "FuncEfe");
        }

        //public bool SendEmail(EnvioEmailModel model)
        //{
        //    _mailService.SendEmail(model.From, model.To, model.Subject, model.Body);
        //    return true;
        //}

        public bool SendEmail(EnvioEmailModel model)
        {
            
            bool checkSent = _mailService.SendEmail(model.From, model.To, model.Subject, model.Body, model.attachmentName);
            //if (!checkSent.IsFaulted && model.attachmentName is not null)
            //{
            //    foreach(var attachment in model.attachmentName)
            //    {
            //        DeleteAttachment(attachment);
            //    }
            //}
            return true;
        }
        
        public bool SaveAttachment(SaveAttachmentModel model)
        {
            return _mailService.SaveAttachment(model.base64, model.fileName);
        }        
        
        public void DeleteAttachment(string attachmentPath)
        {
            _mailService.DeleteAttachment(attachmentPath);
        }
        public string GerarTokenConfirmacao()
        {
            //FALTA PROGRAMAR
            //AQUI CRIARÁ UM TOKEN RANDOMICO E DEPOIS SERÁ INSERIDO NO BANCO
            return "token";
        }
    }

}
