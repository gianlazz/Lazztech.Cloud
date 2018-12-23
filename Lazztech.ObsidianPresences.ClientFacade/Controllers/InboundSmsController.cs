using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackathonManager.DTO;
using HackathonManager.Sms;
using Microsoft.AspNetCore.Mvc;
using Twilio.AspNet.Common;

namespace Lazztech.ObsidianPresences.ClientFacade.Controllers
{
    [Route("api/[controller]")]
    public class InboundSmsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public void Index(SmsRequest request, string To, string From, string Body)
        {
            var smsDto = new SmsDto();
            smsDto.Sid = request.SmsSid;
            smsDto.DateCreated = DateTime.Now;
            smsDto.ToPhoneNumber = To;
            smsDto.FromPhoneNumber = From;
            smsDto.MessageBody = Body;

            var Db = Startup.DbRepo;

            Db.Add<SmsDto>(smsDto);
            SmsRoutingConductor.InboundMessages.Add(smsDto);
        }
    }
}