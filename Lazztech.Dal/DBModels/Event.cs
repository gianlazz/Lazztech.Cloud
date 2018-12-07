using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Dal.DBModels
{
    class Event
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public IList<Person> People { get; set; }
        public IList<Team> Teams { get; set; }
        public Location Location { get; set; }
    }
}
