using HackathonManager.Sms;
using Lazztech.Events.Domain.Sms;
using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using Twilio.AspNet.Common;

namespace Lazztech.Cloud.ClientFacade.Controllers
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
            Startup.RequestConductor.ProcessSmsRequestResponse(smsDto);
        }
    }
}