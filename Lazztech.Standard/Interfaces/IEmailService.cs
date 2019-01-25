using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Standard.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(string toAddress, string subject, string emailBody);
    }
}
