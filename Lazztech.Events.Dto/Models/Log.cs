using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazztech.Events.Dto.Models
{
    public class Log
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get { return DateTime.Now; } }
        public string Details { get; set; }

        public Log()
        {
            Id = Guid.NewGuid();
        }
    }
}
