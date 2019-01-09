using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackathonManager.Models
{
    public class Log
    {
        public DateTime DateTime { get { return DateTime.Now; } }
        public string Details { get; set; }
    }
}
