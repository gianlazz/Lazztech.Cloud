using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Twilio.TwiML;

namespace Lazztech.Cloud.ClientFacade.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwilioController : ControllerBase
    {
        private readonly IRepository _db;

        public TwilioController(IRepository repository)
        {
            _db = repository;
        }

        [HttpGet]
        public TwiML Get(string Id)
        {
            string domainName = Request.HttpContext.Request.GetDisplayUrl().Replace(Request.Path, String.Empty);
            var audio = _db.Single<VoiceUpload>(x => x.Id.ToString() == Id);
            var response = new VoiceResponse();
            response.Play(new Uri(domainName + "/" + audio.FilePath));

            return response;
        }
    }
}