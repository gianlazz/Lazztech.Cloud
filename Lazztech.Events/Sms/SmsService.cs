using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;

namespace HackathonManager
{
    public class SmsService : ISmsService
    {
        //Responsible for newing up an Sms object?
        private readonly ISmsService _smsService;

        public SmsService(ISmsService sms)
        {
            _smsService = sms;
        }

        public SmsDto SendSms(string toPhoneNumber, string messageBody)
        {
            return _smsService.SendSms(toPhoneNumber, messageBody);
        }
    }
}