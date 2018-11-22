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
        string accountSid = TwilioCredentials.accountSid;
        string authToken = TwilioCredentials.authToken;
        string fromNumber = TwilioCredentials.fromTwilioNumber;


        public SmsDto SendSms(string toPhoneNumber, string messageBody)
        {
            string preparedNumber;
            TwilioClient.Init(accountSid, authToken);
            if (toPhoneNumber.StartsWith("+1"))
                preparedNumber = toPhoneNumber;
            else
            {
                preparedNumber = "+1" + toPhoneNumber;
            }

            var to = new PhoneNumber(preparedNumber);
            var message = MessageResource.Create(
                to,
                from: new PhoneNumber($"+1{fromNumber}"),
                body: messageBody);

            SmsDto smsDto = new SmsDto();
            smsDto.DateCreated = DateTime.Now;
            smsDto.ToPhoneNumber = $"+1{toPhoneNumber}";
            smsDto.FromPhoneNumber = $"+1{fromNumber}";
            smsDto.MessageBody = messageBody;
            smsDto.Sid = message.Sid;

            return smsDto;
        }

    }
}
