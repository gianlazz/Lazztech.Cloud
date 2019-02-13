using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lazztech.Cloud.ClientFacade.Data;
using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Twilio.AspNet.Core;
using Twilio.TwiML;

namespace Lazztech.Cloud.ClientFacade.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwilioHelperController : TwilioController
    {
        private readonly ApplicationDbContext _context;

        public TwilioHelperController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Index(string Id)
        {
            //string domainName = Request.HttpContext.Request.GetDisplayUrl().Replace(Request.Path, String.Empty);
            string domainName = @"http://cloud.lazz.tech";
            //string domainName = @"http://01610e09.ngrok.io";
            var audio = await _context.AudioUploads.FirstOrDefaultAsync(x => x.AudioUploadId == int.Parse(Id));
            var response = new VoiceResponse();
            response.Play(new Uri(domainName + audio.FilePath));

            return TwiML(response);
        }
    }
}