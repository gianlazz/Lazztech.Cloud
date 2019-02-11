using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Twilio.AspNet.Core;
using Twilio.TwiML;

namespace Lazztech.Cloud.ClientFacade.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwilioHelperController : TwilioController
    {
        private readonly IDalHelper _db;

        public TwilioHelperController(IDalHelper dal)
        {
            _db = dal;
        }

        [HttpPost]
        public IActionResult Index(string Id)
        {
            //string domainName = Request.HttpContext.Request.GetDisplayUrl().Replace(Request.Path, String.Empty);
            string domainName = @"http://cloud.lazz.tech";
            //string domainName = @"http://01610e09.ngrok.io";
            var guidId = Guid.Parse(Id);
            var audio = _db.Single<VoiceUpload>(x => x.Id == guidId);
            var response = new VoiceResponse();
            response.Play(new Uri(domainName + audio.FilePath));

            return TwiML(response);
        }
    }
}