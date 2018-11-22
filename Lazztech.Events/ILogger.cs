using System;

namespace HackathonManager
{
    public interface ILogger
    {
        void Log(Exception exception);
        void Log(string s);
    }
}