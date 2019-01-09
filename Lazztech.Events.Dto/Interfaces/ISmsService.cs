using Lazztech.Events.Dto.Models;

namespace Lazztech.Events.Dto.Interfaces
{
    public interface ISmsService
    {
        SmsDto SendSms(string toPhoneNumber, string messageBody);
    }
}