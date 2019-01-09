using System;

namespace Lazztech.Events.Dto.Interfaces
{
    public interface ILogger
    {
        void Log(Exception exception);
        void Log(string s);
    }
}