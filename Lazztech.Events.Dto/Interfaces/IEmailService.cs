using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Events.Dto.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(string toAddress, string emailBody);
    }
}
