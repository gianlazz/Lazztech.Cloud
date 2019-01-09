using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Cloud.ClientFacade.Data.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public IList<Mentor> Mentors { get; set; }
        public IList<Judge> Judges { get; set; }
        public IList<Volunteer> Volunteers { get; set; }
        public IList<Team> Teams { get; set; }
        public Location Location { get; set; }
    }
}
