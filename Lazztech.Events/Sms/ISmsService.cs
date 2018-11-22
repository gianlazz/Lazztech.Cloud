using HackathonManager.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonManager.Interfaces
{
    public interface ISmsService
    {
        SmsDto SendSms(string toPhoneNumber, string messageBody);
    }
}
