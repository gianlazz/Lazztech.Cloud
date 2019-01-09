using HackathonManager.DTO;
using HackathonManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;


namespace HackathonManager.Sms
{
    public class TwilioSmsService : ISmsService
    {
        // Find your Account Sid and Auth Token at twilio.com/console
        string _accountSid;
        string _authToken;
        string _fromNumber;

        public TwilioSmsService(string accountSid, string authToken, string fromNumber)
        {
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
