using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace HackathonManager.Sms
{
    public class TwilioSmsService : ISmsService
    {
        // Find your Account Sid and Auth Token at twilio.com/console
        private string _accountSid;

        private string _authToken;
        private string _fromNumber;

        public TwilioSmsService(string accountSid, string authToken, string fromNumber)
        {
            if (string.IsNullOrEmpty(accountSid) || string.IsNullOrEmpty(authToken) || string.IsNullOrEmpty(fromNumber))
                throw new Exception("A TwilioSmsService string param is null or empty");

            _accountSid = accountSid;
            _authToken = authToken;
            _fromNumber = fromNumber;
        }

        public SmsDto SendSms(string toPhoneNumber, string messageBody)
        {
            string preparedNumber;
            TwilioClient.Init(_accountSid, _authToken);
            if (toPhoneNumber.StartsWith("+1"))
                preparedNumber = toPhoneNumber;
            else
            {
                preparedNumber = "+1" + toPhoneNumber;
            }

            var to = new PhoneNumber(preparedNumber);
            var message = MessageResource.Create(
                to,
                from: new PhoneNumber($"+1{_fromNumber}"),
                body: messageBody);

            SmsDto smsDto = new SmsDto();
            smsDto.DateCreated = DateTime.Now;
            smsDto.ToPhoneNumber = $"+1{toPhoneNumber}";
            smsDto.FromPhoneNumber = $"+1{_fromNumber}";
            smsDto.MessageBody = messageBody;
            smsDto.Sid = message.Sid;

            return smsDto;
        }
    }
}