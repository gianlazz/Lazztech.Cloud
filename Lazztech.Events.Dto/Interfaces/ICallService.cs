using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lazztech.Events.Dto.Interfaces
{
    public interface ICallService
    {
        Task PreRecordedCall(string phoneNumber, string filePath);
    }
}
