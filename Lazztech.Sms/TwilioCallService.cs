﻿using Lazztech.Events.Dto.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML;
using Twilio.TwiML.Voice;

namespace Lazztech.Sms
{
    public class TwilioCallService : ICallService
    {
        // Find your Account Sid and Auth Token at twilio.com/console
        private string _accountSid;
        private string _authToken;
        private string _fromNumber;

        public TwilioCallService(string accountSid, string authToken, string fromNumber)
        {
            if (string.IsNullOrEmpty(accountSid) || string.IsNullOrEmpty(authToken) || string.IsNullOrEmpty(fromNumber))
                throw new Exception("A TwilioSmsService string param is null or empty");

            _accountSid = accountSid;
            _authToken = authToken;
            _fromNumber = fromNumber;
        }

        public async System.Threading.Tasks.Task PreRecordedCall(string phoneNumber, string audioUploadId)
        {
            await System.Threading.Tasks.Task.Run(() => 
            {
                TwilioClient.Init(_accountSid, _authToken);
                var url = @"http://cloud.lazz.tech/api/twiliohelper?Id=" + $"{audioUploadId}";
                //var url = @"http://01610e09.ngrok.io/api/twiliohelper?Id=" + $"{audioUploadId}";

                var call = CallResource.Create(
                    url: new Uri(url),
                    to: new Twilio.Types.PhoneNumber($"+1{phoneNumber}"),
                    from: new Twilio.Types.PhoneNumber($"+1{_fromNumber}")
                );
            });
        }
    }
}
