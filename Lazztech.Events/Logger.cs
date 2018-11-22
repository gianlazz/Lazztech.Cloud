using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HackathonManager.RepositoryPattern;
using HackathonManager.Models;

namespace HackathonManager
{
    class Logger : ILogger
    {
        private readonly IRepository _repo;

        public Logger(IRepository repository)
        {
            _repo = repository;
        }

        public void Log(Exception exception)
        {
            _repo.Add<Log>(new Log() { Details = exception.ToString() });
        }

        public void Log(string s)
        {
            _repo.Add<Log>(new Log() { Details = s });
        }
    }
}
