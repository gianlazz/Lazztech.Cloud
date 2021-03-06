﻿using Lazztech.Events.Domain;
using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using Twilio.AspNet.Common;

namespace Lazztech.Cloud.ClientFacade.Controllers
{
    [Route("api/[controller]")]
    public class InboundSmsController : Controller
    {
        private readonly IMentorRequestConductor _conductor;

        public InboundSmsController(IMentorRequestConductor conductor)
        {
            _conductor = conductor;
        }

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

            _conductor.ProcessResponse(smsDto);
        }
    }
}